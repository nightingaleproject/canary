# Helper module for exporting death records as FHIR
module FhirProducerHelper

  # Given death record data, build and return an equivalent FHIR death record bundle
  def self.to_fhir(death_record_data, test_id)
    death_record_data = death_record_data.transform_values! { |value| value['value'] }
    death_record_data.symbolize_keys!

    # Create a new bundle
    fhir_record = FHIR::Bundle.new(
      'resourceType' => 'Bundle',
      'id' => test_id.to_s,
      'type' => 'document',
    )

    # Create the death record composition
    # https://nightingaleproject.github.io/fhir-death-record/#/volume2/Death-Record-Composition
    composition = FHIR::Composition.new

    # Set the composition type
    composition.type = FHIR::CodeableConcept.new(
      'coding' => {
        'code' => '64297-5',
        'display' => 'Death certificate',
        'system' => 'http://loinc.org'
      }
    )

    # Create a section for the composition
    section = FHIR::Composition::Section.new

    # Set section code
    section.code = FHIR::CodeableConcept.new(
      'coding' => {
        'code' => '69453-9',
        'display' => 'Cause of death',
        'system' => 'http://loinc.org'
      }
    )

    # Create and add the decedent (Patient)
    subject = FhirProducerHelper.death_decedent(death_record_data)
    composition.subject = subject.fullUrl
    fhir_record.entry << subject

    # Create and add the certifier (Practitioner)
    author = FhirProducerHelper.death_certifier(death_record_data)
    composition.author = author.fullUrl
    fhir_record.entry << author

    # Create and add the cause of death(s) (Conditions)
    cause_of_deaths = []
    cause_of_deaths << FhirProducerHelper.cause_of_death_condition(death_record_data, 0, subject)
    cause_of_deaths << FhirProducerHelper.cause_of_death_condition(death_record_data, 1, subject)
    cause_of_deaths << FhirProducerHelper.cause_of_death_condition(death_record_data, 2, subject)
    cause_of_deaths << FhirProducerHelper.cause_of_death_condition(death_record_data, 3, subject)
    cause_of_deaths.compact!

    # Add the cause of death references to the compositon section
    cause_of_deaths.each do |cod|
      section.entry << cod.fullUrl
    end

    # Add the cause of deaths themselves to the bundle
    fhir_record.entry.push(*cause_of_deaths)

    # Create the observations
    observations = []
    observations << FhirProducerHelper.actual_or_presumed_date_of_death(death_record_data)
    observations << FhirProducerHelper.autopsy_performed(death_record_data)
    observations << FhirProducerHelper.autopsy_results_available(death_record_data)
    observations << FhirProducerHelper.date_pronounced_dead(death_record_data)
    observations << FhirProducerHelper.death_resulted_from_injury_at_work(death_record_data)
    observations << FhirProducerHelper.manner_of_death(death_record_data)
    observations << FhirProducerHelper.medical_examiner_or_coroner_contacted(death_record_data)
    observations << FhirProducerHelper.timing_of_pregnancy_in_relation_to_death(death_record_data)
    observations << FhirProducerHelper.tobacco_use_contributed_to_death(death_record_data)
    observations.compact!

    # Add the observation references to the compositon section
    observations.each do |obs|
      section.entry << obs.fullUrl
    end

    # Add the observations themselves to the bundle
    fhir_record.entry.push(*observations)

    # Add the section to the composition
    composition.section = section

    # Finally, add the composition itself to the bundle and return the bundle
    fhir_record.entry.unshift(composition) # Make composition the first bundle entry
    fhir_record
  end


  #############################################################################
  # The below section is for building a Patient (the decedent) that is
  # included in a FHIR death record compositon.
  #############################################################################

  # Returns a FHIR entry that covers:
  # https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Death-Record-Decedent
  #
  # This entry contains a FHIR Patient describing Death-Record-Decedent.
  def self.death_decedent(death_record_data)
    # Patient options
    options = {}

    # Decedent name
    name = {}
    name['given'] = [death_record_data[:decedant_legal_name_given]] unless death_record_data[:decedant_legal_name_given].blank?
    unless death_record_data[:middle_name_of_decedent].blank?
      name['given'] = [] unless name['given']
      name['given'] << death_record_data[:middle_name_of_decedent]
    end
    name['family'] = [death_record_data[:decedant_legal_name_last]] unless death_record_data[:decedant_legal_name_last].blank?
    name['use'] = 'official'
    options['name'] = [name] unless name.empty?

    # Decedent D.O.B.
    unless death_record_data[:date_of_birth_year].blank? || death_record_data[:date_of_birth_month].blank? || death_record_data[:date_of_birth_day].blank?
      options['birthDate'] = death_record_data[:date_of_birth_year] + '-' + death_record_data[:date_of_birth_month] + '-' + death_record_data[:date_of_birth_day]
    end

    # Decedent is deceased
    options['deceasedBoolean'] = true

    # Date and time of death
    unless death_record_data[:date_of_death_month].blank? || death_record_data[:date_of_death_day].blank?
      options['deceasedDateTime'] = DateTime.strptime(death_record_data[:date_of_death_year] + '-' + death_record_data[:date_of_death_month] + '-' + death_record_data[:date_of_death_day], '%Y-%m-%d')
    end

    # Decedent's address
    line = death_record_data[:decedents_residence_street_name]
    line += ' ' + death_record_data[:decedents_residence_unit_or_apt_number] unless line.nil? || death_record_data[:decedents_residence_unit_or_apt_number].blank?
    address = {}
    address['line'] = [line] unless line.blank?
    address['city'] = death_record_data[:decedents_residence_city_or_town_name] unless death_record_data[:decedents_residence_city_or_town_name].blank?
    address['state'] = death_record_data[:decedents_residence_state_name] unless death_record_data[:decedents_residence_state_name].blank?
    address['postalCode'] = death_record_data[:decedents_residence_zip_code] unless death_record_data[:decedents_residence_zip_code].blank?
    options['address'] = address

    # Decedent race
    options['extension'] = []
    race_coding = []
    death_record_data.select { |key, value|
      if key.to_s.include? 'decedents_race_'
        display = key.to_s.gsub('decedents_race_', '').humanize.titleize
        race_coding << {
          'display' => display.squish,
          'code' => RACE_ETHNICITY_CODES[display],
          'system' => 'http://hl7.org/fhir/v3/Race'
        } if death_record_data[key] == 'Y'
      end
    }
    options['extension'] << {
      'url' => 'http://hl7.org/fhir/us/core/StructureDefinition/us-core-race',
      'valueCodeableConcept' => {
        'coding' => race_coding
      }
    } unless race_coding.empty?

    # Decedent place of birth
    options['extension'] << {
      'url' => 'http://hl7.org/fhir/StructureDefinition/birthPlace',
      'valueAddress' => FHIR::Address.new({state: death_record_data[:state_us_territory_or_canadian_province_of_birth_literal]}).to_hash
    } if death_record_data[:state_us_territory_or_canadian_province_of_birth_literal]

    # Decedent's mother's maiden name
    options['extension'] << {
      'url' => 'http://hl7.org/fhir/StructureDefinition/patient-mothersMaidenName',
      'valueString' => death_record_data[:mothers_maiden_surname]
    } if death_record_data[:mothers_maiden_surname]

    # Decedent's birth sex
    options['extension'] << {
      'url' => 'http://hl7.org/fhir/us/core/StructureDefinition/us-core-birthsex',
      'valueCode' => death_record_data[:sex]
    } if death_record_data[:sex]

    # Package patient into entry and return
    patient = FHIR::Patient.new(options)
    entry = FHIR::Bundle::Entry.new
    resource_id = SecureRandom.uuid
    entry.fullUrl = "urn:uuid:#{resource_id}"
    entry.resource = patient
    entry
  end


  #############################################################################
  # The below section is for building a Practitioner (the death certifier)
  # that is included in a FHIR death record compositon.
  #############################################################################

  # Returns a FHIR entry that covers:
  # https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Death-Certifier
  #
  # This entry contains a FHIR Practitioner describing Death-Certifier.
  def self.death_certifier(death_record_data)
    # New practitioner
    practitioner = FHIR::Practitioner.new

    extension = FHIR::Extension.new
    extension.url = 'https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/certifier-type'

    # Certifier name
    name = {}
    name['family'] = [death_record_data[:certifiers_last_name]] unless death_record_data[:certifiers_last_name].blank?
    name['given'] = [death_record_data[:certifiers_first_name]] unless death_record_data[:certifiers_first_name].blank?
    unless death_record_data[:certifiers_middle_name].blank?
      name['given'] = [] unless name['given']
      name['given'] << death_record_data[:certifiers_middle_name]
    end
    name['suffix'] = death_record_data[:certifiers_suffix_name] unless death_record_data[:certifiers_suffix_name].blank?
    practitioner.name = FHIR::HumanName.new(name) unless name.empty?

    # Certifier address
    line = death_record_data[:certifier_street_name]
    line += ' ' + death_record_data[:certifier_unit_or_apt_number] unless line.nil? || death_record_data[:certifier_unit_or_apt_number].blank?
    address = {}
    address['line'] = [line] unless line.blank?
    address['city'] = death_record_data[:certifier_city_or_town_name] unless death_record_data[:certifier_city_or_town_name].blank?
    address['state'] = death_record_data[:state_us_territory_or_canadian_province_of_certifier_literal] unless death_record_data[:state_us_territory_or_canadian_province_of_certifier_literal].blank?
    address['postalCode'] = death_record_data[:certifier_zip] unless death_record_data[:certifier_zip].blank?
    practitioner.address = FHIR::Address.new(address) unless address.empty?

    # Qualification
    qualification = FHIR::Practitioner::Qualification.new
    qualification.identifier = FHIR::Identifier.new(
      'use' => 'official'
    )
    qualification.issuer = FHIR::Identifier.new
    practitioner.qualification = qualification

    # Package practitioner into entry and return
    entry = FHIR::Bundle::Entry.new
    resource_id = SecureRandom.uuid
    entry.fullUrl = "urn:uuid:#{resource_id}"
    entry.resource = practitioner
    entry
  end


  #############################################################################
  # The below section is for building Conditions (causes of deaths) that are
  # included in a FHIR death record compositon.
  #############################################################################

  # Returns a FHIR entry that covers:
  # https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Cause-of-Death-Condition
  #
  # This entry contains a FHIR Condition describing Cause-of-Death-Condition.
  def self.cause_of_death_condition(death_record_data, cod_index, subject)
    # New condition
    condition = FHIR::Condition.new

    # Grab cause and onset
    cause = death_record_data[('cause_of_death_part_i_line_' + (cod_index + 97).chr).to_sym]
    onset = death_record_data[('cause_of_death_part_i_interval_line_' + (cod_index + 97).chr).to_sym]

    # Set cause
    narrative = FHIR::Narrative.new
    narrative.status = 'additional' # Narrative may contain additional information not found in structured data
    narrative.div = cause
    condition.text = narrative

    # Set onset if it exists
    condition.onsetString = onset unless onset.blank?

    # Set decedent reference
    condition.subject = subject.fullUrl

    # Package condition into entry and return
    entry = FHIR::Bundle::Entry.new
    resource_id = SecureRandom.uuid
    entry.fullUrl = "urn:uuid:#{resource_id}"
    entry.resource = condition
    entry
  end


  #############################################################################
  # The below section is for building the various Observations that make up
  # a FHIR death record compositon.
  #############################################################################

  # Returns a FHIR entry that covers:
  # https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Actual-Or-Presumed-Date-Of-Death
  #
  # This entry contains a FHIR Observation describing Actual-Or-Presumed-Date-Of-Death.
  def self.actual_or_presumed_date_of_death(death_record_data)
    # Coding informations for this observation (helps figure out which part of the record this is)
    obs_code = {
      code: '81956-5',
      display: 'Date and time of death',
      system: 'http://loinc.org'
    }

    # Grab death record value
    unless death_record_data[:date_of_death_month].blank? || death_record_data[:date_of_death_day].blank?
      value = DateTime.strptime(death_record_data[:date_of_death_year] + '-' + death_record_data[:date_of_death_month] + '-' + death_record_data[:date_of_death_day], '%Y-%m-%d')
    end
    return nil unless value # Don't construct observation if the record doesn't have this value

    # Metadata information for this observation
    obs_meta = {
      'profile' => 'https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Actual-Or-Presumed-Date-Of-Death'
    }

    # Convert Nightingale input to the proper FHIR specific output
    obs_value = {
      type: 'valueDateTime',
      value: value.to_s
    }

    # Construct and return this entry
    FhirProducerHelper.observation(obs_code, obs_value, obs_meta)
  end

  # Returns a FHIR entry that covers:
  # https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Autopsy-Performed
  #
  # This entry contains a FHIR Observation describing Autopsy-Performed.
  def self.autopsy_performed(death_record_data)
    # Coding informations for this observation (helps figure out which part of the record this is)
    obs_code = {
      code: '85699-7',
      display: 'Autopsy was performed',
      system: 'http://loinc.org'
    }

    # Grab death record value
    value = death_record_data[:was_autopsy_performed]
    return nil unless value # Don't construct observation if the record doesn't have this value

    # Metadata information for this observation
    obs_meta = {
      'profile' => 'https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Autopsy-Performed'
    }

    # Convert Nightingale input to the proper FHIR specific output
    lookup = {
      'Y': true,
      'N': false,
    }.stringify_keys
    obs_value = {
      type: 'valueBoolean',
      value: lookup[value]
    }

    # Construct and return this entry
    FhirProducerHelper.observation(obs_code, obs_value, obs_meta)
  end

  # Returns a FHIR entry that covers:
  # https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Autopsy-Results-Available
  #
  # This entry contains a FHIR Observation describing Autopsy-Results-Available.
  def self.autopsy_results_available(death_record_data)
    # Coding informations for this observation (helps figure out which part of the record this is)
    obs_code = {
      code: '69436-4',
      display: 'Autopsy results available',
      system: 'http://loinc.org'
    }

    # Grab death record value
    value = death_record_data[:were_autopsy_findings_available_to_complete_the_cause_of_death]
    return nil unless value # Don't construct observation if the record doesn't have this value

    # Metadata information for this observation
    obs_meta = {
      'profile' => 'https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Autopsy-Results-Available'
    }

    # Convert Nightingale input to the proper FHIR specific output
    lookup = {
      'Y': true,
      'N': false,
    }.stringify_keys
    obs_value = {
      type: 'valueBoolean',
      value: lookup[value]
    }

    # Construct and return this entry
    FhirProducerHelper.observation(obs_code, obs_value, obs_meta)
  end

  # Returns a FHIR entry that covers:
  # https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Date-Pronounced-Dead
  #
  # This entry contains a FHIR Observation describing Date-Pronounced-Dead.
  def self.date_pronounced_dead(death_record_data)
    # Coding informations for this observation (helps figure out which part of the record this is)
    obs_code = {
      code: '80616-6',
      display: 'Date and time pronounced dead',
      system: 'http://loinc.org'
    }

    # Grab death record value
    return nil # Not supported yet
    #value = death_record_loinc[obs_code[:code]]
    #return nil unless value # Don't construct observation if the record doesn't have this value

    # Metadata information for this observation
    obs_meta = {
      'profile' => 'https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Date-Pronounced-Dead'
    }

    # Convert Nightingale input to the proper FHIR specific output
    obs_value = {
      type: 'valueDateTime',
      value: value.to_s
    }

    # Construct and return this entry
    FhirProducerHelper.observation(obs_code, obs_value, obs_meta)
  end

  # Returns a FHIR entry that covers:
  # https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Death-Resulted-From-Injury-At-Work
  #
  # This entry contains a FHIR Observation describing Death-Resulted-From-Injury-At-Work.
  def self.death_resulted_from_injury_at_work(death_record_data)
    # Coding informations for this observation (helps figure out which part of the record this is)
    obs_code = {
      code: '69444-8',
      display: 'Did death result from injury at work',
      system: 'http://loinc.org'
    }

    # Grab death record value
    value = death_record_data[:injury_at_work]
    return nil unless value # Don't construct observation if the record doesn't have this value

    # Metadata information for this observation
    obs_meta = {
      'profile' => 'https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Death-Resulted-From-Injury-At-Work'
    }

    # Convert Nightingale input to the proper FHIR specific output
    lookup = {
      'Y': true,
      'N': false
    }.stringify_keys
    obs_value = {
      type: 'valueBoolean',
      value: lookup[value]
    }

    # Construct and return this entry
    FhirProducerHelper.observation(obs_code, obs_value, obs_meta)
  end

  # Returns a FHIR entry that covers:
  # https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Injury-Leading-To-Death-Associated-Trans
  #
  # This entry contains a FHIR Observation describing Injury-Leading-To-Death-Associated-Trans.
  def self.injury_leading_to_death_associated_trans(death_record_data)
    value = 'Other'

    # Coding informations for this observation (helps figure out which part of the record this is)
    obs_code = {
      code: '69448-9',
      display: 'Injury leading to death associated with transportation event',
      system: 'http://loinc.org'
    }

    # Grab death record value
    return nil # Not supported yet
    #value = death_record_loinc[obs_code[:code]]
    #return nil unless value # Don't construct observation if the record doesn't have this value

    # Metadata information for this observation
    obs_meta = {
      'profile' => 'https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Injury-Leading-To-Death-Associated-Trans'
    }

    # Convert Nightingale input to the proper FHIR specific output
    # See: https://phinvads.cdc.gov/vads/ViewValueSet.action?id=F148DC82-63C3-40B1-A7D2-D7AD78416D4A
    # OID: 2.16.840.1.114222.4.11.6005
    lookup = {
      'Driver/Operator': {concept: '236320001', system: 'http://snomed.info/sct', display: 'Driver/Operator'},
      'Passenger': {concept: '257500003', system: 'http://snomed.info/sct', display: 'Passenger'},
      'Pedestrian': {concept: '257518000', system: 'http://snomed.info/sct', display: 'Pedestrian'},
      'Other': {concept: 'OTH', system: 'http://hl7.org/fhir/v3/NullFlavor', display: 'Other'}
    }.stringify_keys
    obs_value = {
      type: 'valueCodeableConcept',
      code: lookup[value][:concept],
      display: lookup[value][:display],
      system: lookup[value][:system]
    }

    # Construct and return this entry
    FhirProducerHelper.observation(obs_code, obs_value, obs_meta)
  end

  # Returns a FHIR entry that covers:
  # https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Manner-Of-Death
  #
  # This entry contains a FHIR Observation describing Manner-Of-Death.
  def self.manner_of_death(death_record_data)
    # Coding informations for this observation (helps figure out which part of the record this is)
    obs_code = {
      code: '69449-7',
      display: 'Manner of death',
      system: 'http://loinc.org'
    }

    # Grab death record value
    return nil # Not supported yet
    #value = death_record_loinc[obs_code[:code]]
    #return nil unless value # Don't construct observation if the record doesn't have this value

    # Metadata information for this observation
    obs_meta = {
      'profile' => 'https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Manner-Of-Death'
    }

    # Convert Nightingale input to the proper FHIR specific output
    # See: https://phinvads.cdc.gov/vads/ViewValueSet.action?id=0D3864B7-5330-410D-BC91-40C1C704BBA4
    # OID: 2.16.840.1.114222.4.11.6002
    lookup = {
      'Natural': {concept: '38605008', system: 'http://snomed.info/sct', display: 'Natural'},
      'Accident': {concept: '7878000', system: 'http://snomed.info/sct', display: 'Accident'},
      'Suicide': {concept: '44301001', system: 'http://snomed.info/sct', display: 'Suicide'},
      'Homicide': {concept: '27935005', system: 'http://snomed.info/sct', display: 'Homicide'},
      'Pending Investigation': {concept: '185973002', system: 'http://snomed.info/sct', display: 'Pending Investigation'},
      'Could not be determined': {concept: '65037004', system: 'http://snomed.info/sct', display: 'Could not be determined'}
    }.stringify_keys
    obs_value = {
      type: 'valueCodeableConcept',
      code: lookup[value][:concept],
      display: lookup[value][:display],
      system: lookup[value][:system]
    }

    # Construct and return this entry
    FhirProducerHelper.observation(obs_code, obs_value, obs_meta)
  end

  # Returns a FHIR entry that covers:
  # https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Medical-Examiner-Or-Coroner-Contacted
  #
  # This entry contains a FHIR Observation describing Medical-Examiner-Or-Coroner-Contacted.
  def self.medical_examiner_or_coroner_contacted(death_record_data)
    # Coding informations for this observation (helps figure out which part of the record this is)
    obs_code = {
      code: '74497-9',
      display: 'Medical examiner or coroner was contacted',
      system: 'http://loinc.org'
    }

    # Grab death record value
    value = death_record_data[:was_case_referred_to_medical_examiner_coroner]
    return nil unless value # Don't construct observation if the record doesn't have this value

    # Metadata information for this observation
    obs_meta = {
      'profile' => 'https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Medical-Examiner-Or-Coroner-Contacted'
    }

    # Convert Nightingale input to the proper FHIR specific output
    lookup = {
      'Y': true,
      'N': false
    }.stringify_keys
    obs_value = {
      type: 'valueBoolean',
      value: lookup[value]
    }

    # Construct and return this entry
    FhirProducerHelper.observation(obs_code, obs_value, obs_meta)
  end

  # Returns a FHIR entry that covers:
  # https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Timing-Of-Pregnancy-In-Relation-To-Death
  #
  # This entry contains a FHIR Observation describing Timing-Of-Pregnancy-In-Relation-To-Death.
  def self.timing_of_pregnancy_in_relation_to_death(death_record_data)
    # Coding informations for this observation (helps figure out which part of the record this is)
    obs_code = {
      code: '69442-2',
      display: 'Timing of recent pregnancy in relation to death',
      system: 'http://loinc.org'
    }

    # Grab death record value
    return nil # Not supported yet
    #value = death_record_loinc[obs_code[:code]]
    #return nil unless value # Don't construct observation if the record doesn't have this value

    # Metadata information for this observation
    obs_meta = {
      'profile' => 'https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Timing-Of-Pregnancy-In-Relation-To-Death'
    }

    # Convert Nightingale input to the proper FHIR specific output
    # See: https://phinvads.cdc.gov/vads/ViewValueSet.action?id=C763809B-A38D-4113-8E28-126620B76C2F
    # OID: 2.16.840.1.114222.4.11.6003
    lookup = {
      'Not pregnant within past year': {concept: 'PHC1260', system: 'PHIN VS (CDC Local Coding System)', display: 'Not pregnant within past year'},
      'Pregnant at time of death': {concept: 'PHC1261', system: 'PHIN VS (CDC Local Coding System)', display: 'Pregnant at time of death'},
      'Not pregnant, but pregnant within 42 days of death': {concept: 'PHC1262', system: 'PHIN VS (CDC Local Coding System)', display: 'Not pregnant, but pregnant within 42 days of death'},
      'Not pregnant, but pregnant 43 days to 1 year before death': {concept: 'PHC1263', system: 'PHIN VS (CDC Local Coding System)', display: 'Not pregnant, but pregnant 43 days to 1 year before death'},
      'Unknown if pregnant within the past year': {concept: 'PHC1264', system: 'PHIN VS (CDC Local Coding System)', display: 'Unknown if pregnant within the past year'},
    }.stringify_keys
    debugger unless lookup[value]
    obs_value = {
      type: 'valueCodeableConcept',
      code: lookup[value][:concept],
      display: lookup[value][:display],
      system: lookup[value][:system]
    }

    # Construct and return this entry
    FhirProducerHelper.observation(obs_code, obs_value, obs_meta)
  end

  # Returns a FHIR entry that covers:
  # https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Tobacco-Use-Contributed-To-Death
  #
  # This entry contains a FHIR Observation describing Tobacco-Use-Contributed-To-Death.
  def self.tobacco_use_contributed_to_death(death_record_data)
    # Coding informations for this observation (helps figure out which part of the record this is)
    obs_code = {
      code: '69443-0',
      display: 'Did tobacco use contribute to death',
      system: 'http://loinc.org'
    }

    # Grab death record value
    value = death_record_data[:did_tobacco_use_contribute_to_death]
    return nil unless value # Don't construct observation if the record doesn't have this value

    # Metadata information for this observation
    obs_meta = {
      'profile' => 'https://github.com/nightingaleproject/fhir-death-record/StructureDefinition/Tobacco-Use-Contributed-To-Death'
    }

    # Convert Nightingale input to the proper FHIR specific output
    # See: https://phinvads.cdc.gov/vads/ViewValueSet.action?id=FF7F17AE-3D20-473D-9068-E77A08491242
    # OID: 2.16.840.1.114222.4.11.6004
    lookup = {
      'Y': {concept: '373066001', system: 'http://snomed.info/sct', display: 'Yes'},
      'N': {concept: '373067005', system: 'http://snomed.info/sct', display: 'No'}
    }.stringify_keys
    obs_value = {
      type: 'valueCodeableConcept',
      code: lookup[value][:concept],
      display: lookup[value][:display],
      system: lookup[value][:system]
    }

    # Construct and return this entry
    FhirProducerHelper.observation(obs_code, obs_value, obs_meta)
  end

  # FHIR Observation Entry builder
  def self.observation(obs_code, obs_value, obs_meta)
    # New observation
    observation = FHIR::Observation.new

    # Add code (CodeableConcept)
    observation.code = FHIR::CodeableConcept.new(
      'coding' => {
        'code' => obs_code[:code],
        'display' => obs_code[:display],
        'system' => obs_code[:system]
      }
    )

    # Add metadata
    observation.meta = obs_meta

    # Handle type of value
    if obs_value[:type] == 'valueCodeableConcept' # Add valueCodeableConcept (CodeableConcept)
      observation.valueCodeableConcept = FHIR::CodeableConcept.new(
        'coding' => {
          'code' => obs_value[:code],
          'display' => obs_value[:display],
          'system' => obs_value[:system]
        }
      )
    elsif obs_value[:type] == 'valueBoolean' # Add valueBoolean
      observation.valueBoolean = obs_value[:value]
    elsif obs_value[:type] == 'valueDateTime' # Add valueDateTime
      # We need to make sure the date is set as a proper FHIR valueDateTime,
      # thus we need to handle a few situations that might occur given
      # Nightingale user entry.
      # TODO: This needs some polishing...
      date = Date.strptime(obs_value[:value], '%Y-%m-%d') rescue nil # Try date only
      date = Date.parse(obs_value[:value]) unless date # Try regular datetime
      observation.valueDateTime = date.to_datetime.to_s
    end

    # Package obervation into entry and return
    entry = FHIR::Bundle::Entry.new
    resource_id = SecureRandom.uuid
    entry.fullUrl = "urn:uuid:#{resource_id}"
    entry.resource = observation
    entry
  end


  #############################################################################
  # Lookup helpers
  #############################################################################

  MARITAL_STATUS = {
    'Married' => 'M',
    'Married but seperated' => 'M',
    'Widowed' => 'W',
    'Widowed (but not remarried)' => 'W',
    'Divorced (but not remarried)' => 'D',
    'Never married' => 'S',
    'Unknown' => 'UNK',
  }.stringify_keys

  RACE_ETHNICITY_CODES = {
    'White' => '2106-3',
    'Black or African American' => '2054-5',
    'American Indian or Alaskan Native' => '1002-5',
    'Asian' => '2028-5',
    'Asian Indian' => '2029-7',
    'Chinese' => '2034-7',
    'Filipino' => '2036-2',
    'Japanese' => '2039-6',
    'Korean' => '2040-4',
    'Vietnamese' => '2047-9',
    'Native Hawaiian' => '2079-2',
    'Guamanian' => '2087-5',
    'Chamorro' => '2088-3',
    'Samoan' => '2080-0',
    'Other Pacific Islander' => '2500-7'
  }.stringify_keys

end

module DataHelper
  def self.generate_fake_data
    return {
      date_of_death_year: {value: DateTime.now.strftime("%Y"), description: "Date of Death--Year"},
      state_territory_province_code: {value: Faker::Address.state_abbr, description: "State, U.S. Territory or Canadian Province of Death - code"},
      certificate_number: {value: rand(1..999).to_s, description: "Certificate Number"},
      decedant_legal_name_given: {value: Faker::Name.first_name.truncate(50), description: "Decedent's Legal Name--Given"},
      decedant_legal_name_last: {value: Faker::Name.last_name.truncate(50), description: "Decedent's Legal Name--Last"},
      decedant_legal_name_suffix: {value: Faker::Name.suffix, description: "Decedent's Legal Name--Suffix"},
      father_surname: {value: Faker::Name.last_name.truncate(50), description: "Father's Surname"},
      sex: {value: Faker::Demographic.sex[0], description: "Sex"},
      social_security_number: {value: ['123456789', '987654321'].sample, description: "Social Security Number"},
      date_of_birth_year: {value: rand(3.years).seconds.ago.strftime("%Y"), description: "Date of Birth--Year"},
      date_of_birth_month: {value: rand(1..12).to_s.rjust(2, '0'), description: "Date of Birth--Month"},
      date_of_birth_day: {value: rand(1..29).to_s.rjust(2, '0'), description: "Date of Birth--Day"},
      state_us_territory_or_canadian_province_of_birth_code: {value: Faker::Address.state_abbr, description: "State, U.S. Territory or Canadian Province of Birth - code"},
      state_us_territory_or_canadian_province_of_decedents_residence_code: {value: Faker::Address.state_abbr, description: "State, U.S. Territory or Canadian Province of Decedent's residence - code"},
      decedents_residence_inside_city_limits: {value: 'Y', description: "Decedent's Residence--Inside City Limits"},
      marital_status: {value: ['M', 'W'].sample, description: "Marital Status"},
      place_of_death: {value: ['1', '4'].sample, description: "Place of Death"},
      method_of_disposition: {value: ['B', 'C'].sample, description: "Method of Disposition"},
      date_of_death_month: {value: rand(1..12).to_s.rjust(2, '0'), description: "Date of Death--Month"},
      date_of_death_day: {value: rand(1..29).to_s.rjust(2, '0'), description: "Date of Death--Day"},
      time_of_death: {value: '1100', description: "Time of Death"},
      decedents_race_white: {value: ['Y', 'N'].sample, description: "Decedent's Race--White"},
      decedents_race_black_or_african_american: {value: ['Y', 'N'].sample, description: "Decedent's Race--Black or African American"},
      decedents_race_american_indian_or_alaska_native: {value: ['Y', 'N'].sample, description: "Decedent's Race--American Indian or Alaska Native"},
      decedents_race_asian_indian: {value: 'N', description: "Decedent's Race--Asian Indian"},
      decedents_race_chinese: {value: 'N', description: "Decedent's Race--Chinese"},
      decedents_race_filipino: {value: 'N', description: "Decedent's Race--Filipino"},
      decedents_race_japanese: {value: 'N', description: "Decedent's Race--Japanese"},
      decedents_race_korean: {value: 'N', description: "Decedent's Race--Korean"},
      decedents_race_vietnamese: {value: 'N', description: "Decedent's Race--Vietnamese"},
      decedents_race_other_asian: {value: 'N', description: "Decedent's Race--Other Asian"},
      decedents_race_native_hawaiian: {value: 'Y', description: "Decedent's Race--Native Hawaiian"},
      decedents_race_guamanian_or_chamorro: {value: 'N', description: "Decedent's Race--Guamanian or Chamorro"},
      decedents_race_samoan: {value: 'N', description: "Decedent's Race--Samoan"},
      decedents_race_other_pacific_islander: {value: 'N', description: "Decedent's Race--Other Pacific Islander"},
      date_of_registration_year: {value: rand(3.years).seconds.ago.strftime("%Y"), description: "Date of Registration--Year"},
      date_of_registration_month: {value: rand(1..12).to_s.rjust(2, '0'), description: "Date of Registration--Month"},
      date_of_registration_day: {value: rand(1..29).to_s.rjust(2, '0'), description: "Date of Registration--Day"},
      was_autopsy_performed: {value: ['Y', 'N'].sample, description: "Was Autopsy performed"},
      were_autopsy_findings_available_to_complete_the_cause_of_death: {value: ['Y', 'N'].sample, description: "Were Autopsy Findings Available to Complete the Cause of Death?"},
      did_tobacco_use_contribute_to_death: {value: ['Y', 'N'].sample, description: "Did Tobacco Use Contribute to Death?"},
      date_of_injury_month: {value: rand(1..12).to_s.rjust(2, '0'), description: "Date of injury--month"},
      date_of_injury_day: {value: rand(1..29).to_s.rjust(2, '0'), description: "Date of injury--day"},
      date_of_injury_year: {value: rand(3.years).seconds.ago.strftime("%Y"), description: "Date of injury--year"},
      time_of_injury: {value: '1000', description: "Time of injury"},
      injury_at_work: {value: ['Y', 'N'].sample, description: "Injury at work"},
      time_of_injury_unit: {value: 'A', description: "Time of Injury Unit"},
      decedent_ever_served_in_armed_forces: {value: ['Y', 'N'].sample, description: "Decedent ever served in Armed Forces?"},
      death_institution_name: {value: Faker::Company.name.truncate(20), description: "Death Institution name"},
      place_of_death_street_number: {value: rand(1..999).to_s, description: "Place of death. Street number"},
      place_of_death_street_name: {value: Faker::Address.street_name, description: "Place of death. Street name"},
      place_of_death_street_designator: {value: 'St', description: "Place of death. Street designator"},
      place_of_death_city_or_town_name: {value: Faker::Address.city, description: "Place of death. City or Town name"},
      place_of_death_state_name_literal: {value: Faker::Address.state, description: "Place of death. State name literal"},
      place_of_death_zip_code: {value: 5.times.map{rand(5)}.join, description: "Place of death. Zip code"},
      place_of_death_county_of_death: {value: Faker::Address.city, description: "Place of death. County of Death"},
      spouses_first_name: {value: Faker::Name.first_name.truncate(50), description: "Spouse's First Name"},
      husbands_surname_wifes_maiden_last_name: {value: Faker::Name.last_name.truncate(50), description: "Husband's Surname/Wife's Maiden Last Name"},
      decedents_residence_street_number: {value: rand(1..999).to_s, description: "Decedent's Residence - Street number"},
      decedents_residence_street_name: {value: Faker::Address.street_name, description: "Decedent's Residence - Street name"},
      decedents_residence_street_designator: {value: 'St', description: "Decedent's Residence - Street designator"},
      decedents_residence_city_or_town_name: {value: Faker::Address.city, description: "Decedent's Residence - City or Town name"},
      decedents_residence_zip_code: {value: 5.times.map{rand(10)}.join, description: "Decedent's Residence - ZIP code"},
      decedents_residence_state_name: {value: Faker::Address.state, description: "Decedent's Residence - State name"},
      middle_name_of_decedent: {value: Faker::Name.first_name.truncate(50), description: "Middle Name of Decedent"},
      fathers_first_name: {value: Faker::Name.first_name.truncate(50), description: "Father's First Name"},
      fathers_middle_name: {value: Faker::Name.first_name.truncate(50), description: "Father's Middle Name"},
      mothers_first_name: {value: Faker::Name.first_name.truncate(50), description: "Mother's First Name"},
      mothers_middle_name: {value: Faker::Name.first_name.truncate(50), description: "Mother's Middle Name"},
      mothers_maiden_surname: {value: Faker::Name.last_name.truncate(50), description: "Mother's Maiden Surname"},
      was_case_referred_to_medical_examiner_coroner: {value: 'N', description: "Was case Referred to Medical Examiner/Coroner?"},
      describe_how_injury_occurred: {value: Faker::Lorem.sentence, description: "Describe How Injury Occurred"},
      county_of_injury_literal: {value: Faker::Address.city, description: "County of Injury - literal"},
      town_city_of_injury_literal: {value: Faker::Address.city, description: "Town/city of Injury - literal"},
      state_us_territory_or_canadian_province_of_injury_code: {value: Faker::Address.state_abbr, description: "State, U.S. Territory or Canadian Province of Injury - code"},
      cause_of_death_part_i_line_a: {value: 'Rupture of myocardium', description: "Cause of Death Part I Line a"},
      cause_of_death_part_i_interval_line_a: {value: "#{rand(1..59)} minutes", description: "Cause of Death Part I Interval, Line a"},
      cause_of_death_part_i_line_b: {value: 'Acute myocardial infarction', description: "Cause of Death Part I Line b"},
      cause_of_death_part_i_interval_line_b: {value: "#{rand(1..59)} days", description: "Cause of Death Part I Interval, Line b"},
      cause_of_death_part_i_line_c: {value: 'Coronary artery thrombosis', description: "Cause of Death Part I Line c"},
      cause_of_death_part_i_interval_line_c: {value: "#{rand(1..59)} days", description: "Cause of Death Part I Interval, Line c"},
      cause_of_death_part_i_line_d: {value: 'Atherosclerotic coronary artery disease', description: "Cause of Death Part I Line d"},
      cause_of_death_part_i_interval_line_d: {value: "#{rand(1..59)} years", description: "Cause of Death Part I Interval, Line d"},
      spouses_middle_name: {value: Faker::Name.first_name.truncate(50), description: "Spouse's Middle Name"},
      spouses_suffix: {value: Faker::Name.suffix, description: "Spouse's Suffix"},
      fathers_suffix: {value: Faker::Name.suffix, description: "Father's Suffix"},
      mothers_suffix: {value: Faker::Name.suffix, description: "Mother's Suffix"},
      informants_relationship: {value: 'Relative', description: "Informant's Relationship"},
      state_us_territory_or_canadian_province_of_disposition_code: {value: Faker::Address.state_abbr, description: "State, U.S. Territory or Canadian Province of Disposition - code"},
      disposition_state_or_territory_literal: {value: Faker::Address.state, description: "Disposition State or Territory - Literal"},
      funeral_facility_name: {value: Faker::Company.name, description: "Funeral Facility Name"},
      funeral_facility_street_number: {value: rand(1..999).to_s, description: "Funeral Facility - Street number"},
      funeral_facility_street_name: {value: Faker::Address.street_name, description: "Funeral Facility - Street name"},
      funeral_facility_street_designator: {value: 'St', description: "Funeral Facility - Street designator"},
      funeral_facility_city_or_town_name: {value: Faker::Address.city, description: "Funeral Facility - City or Town name"},
      state_us_territory_or_canadian_province_of_funeral_facility_code: {value: Faker::Address.state_abbr, description: "State, U.S. Territory or Canadian Province of Funeral Facility - code"},
      state_us_territory_or_canadian_province_of_funeral_facility_literal: {value: Faker::Address.state, description: "State, U.S. Territory or Canadian Province of Funeral Facility - literal"},
      funeral_facility_zip: {value: 5.times.map{rand(10)}.join, description: "Funeral Facility - ZIP"},
      certifiers_first_name: {value: Faker::Name.first_name.truncate(50), description: "Certifier's First Name"},
      certifiers_middle_name: {value: Faker::Name.first_name.truncate(50), description: "Certifier's Middle Name"},
      certifiers_last_name: {value: Faker::Name.last_name.truncate(50), description: "Certifier's Last Name"},
      certifiers_suffix_name: {value: Faker::Name.suffix, description: "Certifier's Suffix Name"},
      certifier_street_number: {value: rand(1..999).to_s, description: "Certifier - Street number"},
      certifier_street_name: {value: Faker::Address.street_name, description: "Certifier - Street name"},
      certifier_street_designator: {value: 'St', description: "Certifier - Street designator"},
      certifier_city_or_town_name: {value: Faker::Address.city, description: "Certifier - City or Town name"},
      state_us_territory_or_canadian_province_of_certifier_code: {value: Faker::Address.state_abbr, description: "State, U.S. Territory or Canadian Province of Certifier - code"},
      state_us_territory_or_canadian_province_of_certifier_literal: {value: Faker::Address.state, description: "State, U.S. Territory or Canadian Province of Certifier - literal"},
      certifier_zip: {value: 5.times.map{rand(10)}.join, description: "Certifier - Zip"},
      certifier_date_signed: {value: DateTime.now.strftime("%m%d%Y"), description: "Certifier Date Signed"},
      date_filed: {value: DateTime.now.strftime("%m%d%Y"), description: "Date Filed"},
      state_us_territory_or_canadian_province_of_injury_literal: {value: Faker::Address.state_abbr, description: "State, U.S. Territory or Canadian Province of Injury - literal"},
      state_us_territory_or_canadian_province_of_birth_literal: {value: Faker::Address.state, description: "State, U.S. Territory or Canadian Province of Birth - literal"},
      occupation_literal_optional: {value: 'Example' + rand(1..999).to_s, description: "Decedent Usual Occupation"},
      pregnancy: {value: ['1', '9'].sample, description: "Timing of recent pregnancy in relation to death"},
      title_of_certifier: {value: ['D', 'M'].sample, description: "Certifier Type"},
      decedents_education: {value: ['6', '7'].sample, description: "Decedent's Education Level"},
      industry_literal_optional: {value: 'Example' + rand(1..999).to_s, description: "Decedent Kind Of Industry"},
      manner_of_death: {value: 'N', description: "Manner of Death"},
    }
  end

  # Generate a subset of fake data, specifically for FHIR use.
  def self.generate_fake_data_fhir
    DataHelper.generate_fake_data.slice(:decedent_ever_served_in_armed_forces,
                                        :were_autopsy_findings_available_to_complete_the_cause_of_death,
                                        :was_autopsy_performed,
                                        :title_of_certifier,
                                        :cause_of_death_part_i_line_a,
                                        :cause_of_death_part_i_interval_line_a,
                                        :cause_of_death_part_i_line_b,
                                        :cause_of_death_part_i_interval_line_b,
                                        :cause_of_death_part_i_line_c,
                                        :cause_of_death_part_i_interval_line_c,
                                        :cause_of_death_part_i_line_d,
                                        :cause_of_death_part_i_interval_line_d,
                                        :date_of_birth_year,
                                        :date_of_birth_month,
                                        :date_of_birth_day,
                                        :date_of_death_year,
                                        :date_of_death_month,
                                        :date_of_death_day,
                                        :injury_at_work,
                                        :decedents_residence_city_or_town_name,
                                        :decedents_residence_state_name,
                                        :decedents_residence_zip_code,
                                        :decedant_legal_name_given,
                                        :decedant_legal_name_last,
                                        :did_tobacco_use_contribute_to_death,
                                        :decedents_education,
                                        :funeral_facility_name,
                                        :funeral_facility_city_or_town_name,
                                        :state_us_territory_or_canadian_province_of_funeral_facility_literal,
                                        :funeral_facility_zip,
                                        :industry_literal_optional,
                                        :place_of_death_city_or_town_name,
                                        :place_of_death_state_name,
                                        :place_of_death_zip_code,
                                        :place_of_death_state_name_literal,
                                        :marital_status,
                                        :was_case_referred_to_medical_examiner_coroner,
                                        :method_of_disposition,
                                        :mothers_maiden_surname,
                                        :certifier_city_or_town_name,
                                        :state_us_territory_or_canadian_province_of_certifier_literal,
                                        :certifier_zip,
                                        :certifiers_first_name,
                                        :certifiers_last_name,
                                        :certifiers_middle_name,
                                        :state_us_territory_or_canadian_province_of_birth_literal,
                                        :place_of_death,
                                        :disposition_state_or_territory_literal,
                                        :pregnancy,
                                        :sex,
                                        :social_security_number,
                                        :occupation_literal_optional)
  end

  # Create mappings from that fake data for use by ruby-fhir-death-record structure
  def self.generate_fake_data_fhir_mappings(fake_data)
    fake_data = Hash[ fake_data.collect { |k, v| [k, v['value']] } ]
    fake_data.symbolize_keys!
    {
      "armedForcesService.armedForcesService": DataHelper.to_yes_no(fake_data[:decedent_ever_served_in_armed_forces]),
      "autopsyAvailableToCompleteCauseOfDeath.autopsyAvailableToCompleteCauseOfDeath": DataHelper.to_yes_no(fake_data[:were_autopsy_findings_available_to_complete_the_cause_of_death]),
      "autopsyPerformed.autopsyPerformed": DataHelper.to_yes_no(fake_data[:was_autopsy_performed]),
      "certifierType.certifierType": DataHelper.to_certifier_type(fake_data[:title_of_certifier]),
      "cod.immediate": fake_data[:cause_of_death_part_i_line_a],
      "cod.immediateInt": fake_data[:cause_of_death_part_i_interval_line_a],
      "cod.under1": fake_data[:cause_of_death_part_i_line_b],
      "cod.under1Int": fake_data[:cause_of_death_part_i_interval_line_b],
      "cod.under2": fake_data[:cause_of_death_part_i_line_c],
      "cod.under2Int": fake_data[:cause_of_death_part_i_interval_line_c],
      "cod.under3": fake_data[:cause_of_death_part_i_line_d],
      "cod.under3Int": fake_data[:cause_of_death_part_i_interval_line_d],
      "dateOfBirth.dateOfBirth": "#{fake_data[:date_of_birth_year]}-#{fake_data[:date_of_birth_month]}-#{fake_data[:date_of_birth_day]}",
      "dateOfDeath.dateOfDeath": "#{fake_data[:date_of_death_year]}-#{fake_data[:date_of_death_month]}-#{fake_data[:date_of_death_day]}",
      "deathResultedFromInjuryAtWork.deathResultedFromInjuryAtWork": DataHelper.to_yes_no(fake_data[:injury_at_work]),
      "decedentAddress.city": fake_data[:decedents_residence_city_or_town_name],
      "decedentAddress.state": fake_data[:decedents_residence_state_name],
      "decedentAddress.zip": fake_data[:decedents_residence_zip_code],
      "decedentName.firstName": fake_data[:decedant_legal_name_given],
      "decedentName.lastName": fake_data[:decedant_legal_name_last],
      "didTobaccoUseContributeToDeath.didTobaccoUseContributeToDeath": DataHelper.to_yes_no(fake_data[:did_tobacco_use_contribute_to_death]),
      "education.education": DataHelper.to_education(fake_data[:decedents_education]),
      "funeralFacility.name": fake_data[:funeral_facility_name],
      "funeralFacility.city": fake_data[:funeral_facility_city_or_town_name],
      "funeralFacility.state": fake_data[:state_us_territory_or_canadian_province_of_funeral_facility_literal],
      "funeralFacility.zip": fake_data[:funeral_facility_zip],
      "hispanicOrigin.hispanicOrigin.option": "No",
      "hispanicOrigin.hispanicOrigin.specify": "",
      "kindOfBusiness.kindOfBusiness": fake_data[:industry_literal_optional],
      "locationOfDeath.city": fake_data[:place_of_death_city_or_town_name],
      "locationOfDeath.state": fake_data[:place_of_death_state_name_literal],
      "locationOfDeath.zip": fake_data[:place_of_death_zip_code],
      "mannerOfDeath.mannerOfDeath": "Natural",
      "maritalStatus.maritalStatus": DataHelper.to_marital_status(fake_data[:marital_status]),
      "meOrCoronerContacted.meOrCoronerContacted": DataHelper.to_yes_no(fake_data[:was_case_referred_to_medical_examiner_coroner]),
      "methodOfDisposition.methodOfDisposition.option": DataHelper.to_method_of_disposition(fake_data[:method_of_disposition]),
      "motherName.lastName": fake_data[:mothers_maiden_surname],
      "personCompletingCauseOfDeathAddress.city": fake_data[:certifier_city_or_town_name],
      "personCompletingCauseOfDeathAddress.state": fake_data[:state_us_territory_or_canadian_province_of_certifier_literal],
      "personCompletingCauseOfDeathAddress.zip": fake_data[:certifier_zip],
      "personCompletingCauseOfDeathName.firstName": fake_data[:certifiers_first_name],
      "personCompletingCauseOfDeathName.lastName": fake_data[:certifiers_last_name],
      "personCompletingCauseOfDeathName.middleName": fake_data[:certifiers_middle_name],
      "placeOfBirth.state": fake_data[:state_us_territory_or_canadian_province_of_birth_literal],
      "placeOfDeath.placeOfDeath.option": DataHelper.to_place_of_death(fake_data[:place_of_death]),
      "placeOfDisposition.state": fake_data[:disposition_state_or_territory_literal],
      "pregnancyStatus.pregnancyStatus": DataHelper.to_pregnancy(fake_data[:pregnancy]),
      "race.race.option": "Known",
      "race.race.specify": DataHelper.to_race(fake_data),
      "sex.sex": DataHelper.to_male_female(fake_data[:sex]),
      "ssn.ssn1": fake_data[:social_security_number][0..2],
      "ssn.ssn2": fake_data[:social_security_number][3..4],
      "ssn.ssn3": fake_data[:social_security_number][5..8],
      "usualOccupation.usualOccupation": fake_data[:occupation_literal_optional]
    }
  end

  def self.to_yes_no(val)
    val == 'Y' ? 'Yes' : 'No'
  end

  def self.to_time(val)
    debugger
    "#{val.to_s[0..1]}:#{val.to_s[2..3]}"
  end

  def self.to_male_female(val)
    if val == 'M'
      'Male'
    elsif val == 'F'
      'Female'
    else
      'Unkown'
    end
  end

  def self.to_race(fake_data)
    races = []
    races << 'Native Hawaiian or Other Pacific Islander' if fake_data[:decedents_race_native_hawaiian] == 'Y'
    races << 'White' if fake_data[:decedents_race_white] == 'Y'
    races << 'Black or African American' if fake_data[:decedents_race_black_or_african_american] == 'Y'
    races << 'American Indian or Alaskan Native' if fake_data[:decedents_race_american_indian_or_alaska_native] == 'Y'
    races.to_json.to_s
  end

  def self.to_pregnancy(val)
    val.to_s == '1' ? 'Not pregnant within past year' : 'Unknown if pregnant within the past year'
  end

  def self.to_certifier_type(val)
    val.to_s == 'D' ? 'Physician (Certifier)' : 'Medical Examiner'
  end

  def self.to_education(val)
    val.to_s == '6' ? "Bachelor's Degree" : "Master's Degree"
  end

  def self.to_marital_status(val)
    val.to_s == 'M' ? 'Married' : 'Widowed'
  end

  def self.to_method_of_disposition(val)
    val.to_s == 'B' ? 'Burial' : 'Cremation'
  end

  def self.to_place_of_death(val)
    val.to_s == '1' ? 'Death in hospital' : 'Death in home'
  end

  def self.to_full_state(val)
    {
      "Alabama" => "AL",
    	"Alaska" => "AK",
      "American Samoa" => "AS",
    	"Arizona" => "AZ",
    	"Arkansas" => "AR",
    	"California" => "CA",
    	"Colorado" => "CO",
    	"Connecticut" => "CT",
    	"Delaware" => "DE",
    	"District of Columbia" => "DC",
    	"Florida" => "FL",
    	"Georgia" => "GA",
      "Guam" => "GU",
    	"Hawaii" => "HI",
    	"Idaho" => "ID",
    	"Illinois" => "IL",
    	"Indiana" => "IN",
    	"Iowa" => "IA",
    	"Kansas" => "KS",
    	"Kentucky" => "KY",
    	"Louisiana" => "LA",
    	"Maine" => "ME",
    	"Maryland" => "MD",
    	"Massachusetts" => "MA",
    	"Michigan" => "MI",
    	"Minnesota" => "MN",
    	"Mississippi" => "MS",
    	"Missouri" => "MO",
    	"Montana" => "MT",
    	"Nebraska" => "NE",
    	"Nevada" => "NV",
    	"New Hampshire" => "NH",
    	"New Jersey" => "NJ",
    	"New Mexico" => "NM",
    	"New York" => "NY",
      "New York City" => "YC",
    	"North Carolina" => "NC",
    	"North Dakota" => "ND",
      "Northern Mariana Islands" => "MP",
    	"Ohio" => "OH",
    	"Oklahoma" => "OK",
    	"Oregon" => "OR",
    	"Pennsylvania" => "PA",
      "Puerto Rico" => "PR",
    	"Rhode Island" => "RI",
    	"South Carolina" => "SC",
    	"South Dakota" => "SD",
    	"Tennessee" => "TN",
    	"Texas" => "TX",
    	"Utah" => "UT",
    	"Vermont" => "VT",
      "Virgin Islands" => "VI",
    	"Virginia" => "VA",
    	"Washington" => "WA",
    	"West Virginia" => "WV",
    	"Wisconsin" => "WI",
    	"Wyoming" => "WY",
      "British Columbia" => "BC",
      "Ontario" => "ON",
      "Newfoundland and Labrador" => "NL",
      "Northwest Territories" => "NT",
      "Nova Scotia" => "NS",
      "Prince Edward Island" => "PE",
      "New Brunswick" => "NB",
      "Quebec" => "QC",
      "Manitoba" => "MB",
      "Saskatchewan" => "SK",
      "Alberta" => "AB",
      "Nunavut" => "NU",
      "Yukon" => "YT",
      "Unknown Territory" => "ZZ",
      "Country is Known But Not U.S. Or Canada" => "XX"
    }.key(val)
  end
end

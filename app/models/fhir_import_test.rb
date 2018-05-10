class FhirImportTest < ApplicationRecord
  default_scope { order('created_at DESC') }

  belongs_to :system, class_name: 'System'

  def populate_fake_data
    self.data = DataHelper.generate_fake_data_fhir
    self.fhir_mappings = DataHelper.generate_fake_data_fhir_mappings(self.data)
    self.fhir_data = FhirDeathRecord::Producer.to_fhir({'contents': self.fhir_mappings, id: '1234567890', certifier_id: '1234567890'})
    self.save
  end
end

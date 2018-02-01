class FhirExportTest < ApplicationRecord
  default_scope { order('created_at DESC') }

  belongs_to :system, class_name: 'System'

  def populate_fake_data
    self.data = DataHelper.generate_fake_data_fhir
    self.save
  end
end

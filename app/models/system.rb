class System < ApplicationRecord
  belongs_to :creator, class_name: 'User'

  has_many :ije_export_tests, class_name: 'IjeExportTest', foreign_key: 'system_id'
  has_many :fhir_export_tests, class_name: 'FhirExportTest', foreign_key: 'system_id'
  has_many :ije_import_tests, class_name: 'IjeImportTest', foreign_key: 'system_id'
  has_many :fhir_import_tests, class_name: 'FhirImportTest', foreign_key: 'system_id'
end

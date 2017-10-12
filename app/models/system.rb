class System < ApplicationRecord
  belongs_to :creator, class_name: 'User'

  has_many :ije_export_results, class_name: 'IjeExportResult', foreign_key: 'system_id'
  has_many :fhir_export_results, class_name: 'FhirExportResult', foreign_key: 'system_id'
  has_many :cda_export_results, class_name: 'CdaExportResult', foreign_key: 'system_id'
  has_many :ije_import_results, class_name: 'IjeImportResult', foreign_key: 'system_id'
  has_many :fhir_import_results, class_name: 'FhirImportResult', foreign_key: 'system_id'
  has_many :cda_import_results, class_name: 'CdaImportResult', foreign_key: 'system_id'
end

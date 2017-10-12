class FhirImportResult < ApplicationRecord
  default_scope { order('created_at DESC') }
  
  belongs_to :system, class_name: 'System'
end

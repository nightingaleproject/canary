class CreateFhirImportTests < ActiveRecord::Migration[5.1]
  def change
    create_table :fhir_import_tests do |t|
      t.belongs_to :system, class_name: 'System'

      t.integer :score, default: 0
      t.boolean :complete, default: false
      t.json :data, default: {}
      t.json :fhir_mappings, default: {}
      t.json :fhir_data, default: {}
      t.json :ije_to_fhir_paths, default: {}
      t.json :problems, default: {}
      t.json :successes, default: {}
      t.string :filename, default: ''

      t.timestamps
    end
  end
end

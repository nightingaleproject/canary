class CreateFhirExportTests < ActiveRecord::Migration[5.1]
  def change
    create_table :fhir_export_tests do |t|
      t.belongs_to :system, class_name: 'System'

      t.integer :score, default: 0
      t.boolean :complete, default: false
      t.json :data, default: {}
      t.json :fhir_mappings, default: {}
      t.text :diffs, default: ''
      t.text :good_fhir, default: ''
      t.string :filename, default: ''

      t.timestamps
    end
  end
end

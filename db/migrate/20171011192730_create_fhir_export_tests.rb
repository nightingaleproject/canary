class CreateFhirExportTests < ActiveRecord::Migration[5.1]
  def change
    create_table :fhir_export_tests do |t|
      t.belongs_to :system, class_name: 'System'

      t.integer :score, default: 0

      t.timestamps
    end
  end
end

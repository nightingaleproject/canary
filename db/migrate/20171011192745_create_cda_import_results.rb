class CreateCdaImportResults < ActiveRecord::Migration[5.1]
  def change
    create_table :cda_import_results do |t|
      t.belongs_to :system, class_name: 'System'

      t.integer :score

      t.timestamps
    end
  end
end

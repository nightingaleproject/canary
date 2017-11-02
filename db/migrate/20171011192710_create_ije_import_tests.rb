class CreateIjeImportTests < ActiveRecord::Migration[5.1]
  def change
    create_table :ije_import_tests do |t|
      t.belongs_to :system, class_name: 'System'

      t.integer :score, default: 0
      t.boolean :complete, default: false
      t.json :data, default: {}
      t.string :filename, default: ''

      t.timestamps
    end
  end
end

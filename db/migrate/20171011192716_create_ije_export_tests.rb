class CreateIjeExportTests < ActiveRecord::Migration[5.1]
  def change
    create_table :ije_export_tests do |t|
      t.belongs_to :system, class_name: 'System'

      t.integer :score, default: 0
      t.boolean :complete, default: false
      t.json :data, default: {}
      t.json :problems, default: {}
      t.text :ije, default: ''

      t.timestamps
    end
  end
end

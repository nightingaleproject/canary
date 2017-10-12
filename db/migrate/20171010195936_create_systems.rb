class CreateSystems < ActiveRecord::Migration[5.1]
  def change
    create_table :systems do |t|
      t.belongs_to :creator, class_name: 'User'

      t.string :name
      t.text :notes

      t.timestamps
    end
  end
end

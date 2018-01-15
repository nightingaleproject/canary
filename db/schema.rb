# This file is auto-generated from the current state of the database. Instead
# of editing this file, please use the migrations feature of Active Record to
# incrementally modify your database, and then regenerate this schema definition.
#
# Note that this schema.rb definition is the authoritative source for your
# database schema. If you need to create the application database on another
# system, you should be using db:schema:load, not running all the migrations
# from scratch. The latter is a flawed and unsustainable approach (the more migrations
# you'll amass, the slower it'll run and the greater likelihood for issues).
#
# It's strongly recommended that you check this file into your version control system.

ActiveRecord::Schema.define(version: 20171011192745) do

  # These are extensions that must be enabled in order to support this database
  enable_extension "plpgsql"

  create_table "cda_export_tests", force: :cascade do |t|
    t.bigint "system_id"
    t.integer "score", default: 0
    t.datetime "created_at", null: false
    t.datetime "updated_at", null: false
    t.index ["system_id"], name: "index_cda_export_tests_on_system_id"
  end

  create_table "cda_import_tests", force: :cascade do |t|
    t.bigint "system_id"
    t.integer "score", default: 0
    t.datetime "created_at", null: false
    t.datetime "updated_at", null: false
    t.index ["system_id"], name: "index_cda_import_tests_on_system_id"
  end

  create_table "fhir_export_tests", force: :cascade do |t|
    t.bigint "system_id"
    t.integer "score", default: 0
    t.datetime "created_at", null: false
    t.datetime "updated_at", null: false
    t.index ["system_id"], name: "index_fhir_export_tests_on_system_id"
  end

  create_table "fhir_import_tests", force: :cascade do |t|
    t.bigint "system_id"
    t.integer "score", default: 0
    t.boolean "complete", default: false
    t.json "data", default: {}
    t.json "problems", default: {}
    t.json "successes", default: {}
    t.string "filename", default: ""
    t.datetime "created_at", null: false
    t.datetime "updated_at", null: false
    t.index ["system_id"], name: "index_fhir_import_tests_on_system_id"
  end

  create_table "ije_export_tests", force: :cascade do |t|
    t.bigint "system_id"
    t.integer "score", default: 0
    t.boolean "complete", default: false
    t.json "data", default: {}
    t.json "problems", default: {}
    t.json "successes", default: {}
    t.text "ije", default: ""
    t.datetime "created_at", null: false
    t.datetime "updated_at", null: false
    t.index ["system_id"], name: "index_ije_export_tests_on_system_id"
  end

  create_table "ije_import_tests", force: :cascade do |t|
    t.bigint "system_id"
    t.integer "score", default: 0
    t.boolean "complete", default: false
    t.json "data", default: {}
    t.string "filename", default: ""
    t.datetime "created_at", null: false
    t.datetime "updated_at", null: false
    t.index ["system_id"], name: "index_ije_import_tests_on_system_id"
  end

  create_table "systems", force: :cascade do |t|
    t.bigint "creator_id"
    t.string "name"
    t.text "notes"
    t.datetime "created_at", null: false
    t.datetime "updated_at", null: false
    t.index ["creator_id"], name: "index_systems_on_creator_id"
  end

  create_table "users", force: :cascade do |t|
    t.string "email", default: "", null: false
    t.string "encrypted_password", default: "", null: false
    t.integer "sign_in_count", default: 0, null: false
    t.datetime "current_sign_in_at"
    t.datetime "last_sign_in_at"
    t.inet "current_sign_in_ip"
    t.inet "last_sign_in_ip"
    t.string "first_name", default: "", null: false
    t.string "last_name", default: "", null: false
    t.datetime "created_at", null: false
    t.datetime "updated_at", null: false
    t.index ["email"], name: "index_users_on_email", unique: true
  end

end

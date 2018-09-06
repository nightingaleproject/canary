require 'rest-client'
require 'fhirdeathrecord'

class TestController < ApplicationController
  before_action :new_test, only: [:new]
  before_action :set_test, only: [:show, :upload, :upload_fhir, :download_ije, :submit_checks, :send_fhir]
  before_action :set_tests, only: [:index]

  # Show all tests.
  def index
  end

  # Show specific test.
  def show
    @type = params['test_type']
  end

  # Create new test.
  def new
    redirect_to test_path @test, system_id: params['system_id'], test_type: params['test_type']
  end

  def download_ije
    send_file "#{Rails.root}/public/" + @test.filename, x_sendfile: true
  end

  def send_fhir
    target = params[:endpoint]

    # Generate FHIR from test fake data
    fhir = @test.fhir_data

    # Post FHIR to given endpoint
    # TODO: Assuming certain param structure (for Nightingale); also assuming JSON!
    # TODO: Need to handle 404s, 500s, etc (and tell the user).
    RestClient.post target, fhir.to_json, {content_type: :json}
  end

  def submit_checks
    good = params.keys.select { |k| k.include? 'canary-option' }.count
    @test.complete = true
    @test.score = ((good.to_f / @test.data.keys.count.to_f) * 100).to_i
    @test.save
    redirect_to test_path @test, system_id: params['system_id'], test_type: params['test_type']
  end

  def upload_fhir
    uploaded_io = params[:fhir]
    File.open(Rails.root.join('public', 'uploads', uploaded_io.original_filename), 'wb') do |file|
      file.write(uploaded_io.read)
    end
    doc1 = Nokogiri::XML(File.open(Rails.root.join('public', 'uploads', uploaded_io.original_filename))).to_xml(indent: 3, indent_text: ' ')
    doc2 = Nokogiri::XML(@test.good_fhir).to_xml(indent: 3, indent_text: ' ')
    diffs = Diffy::Diff.new(doc2, doc1, :include_plus_and_minus_in_html => true)
    @test.diffs = diffs.to_s(:html)
    @test.complete = true
    @test.score = if diffs.count == 0 then 100 else ((diffs.count.to_f / (doc1.lines.count.to_f + doc2.lines.count.to_f)) * 100).to_i end
    @test.save
    redirect_to test_path @test, system_id: params['system_id'], test_type: params['test_type']
  end

  def upload
    uploaded_io = params[:ije]
    File.open(Rails.root.join('public', 'uploads', uploaded_io.original_filename), 'wb') do |file|
      file.write(uploaded_io.read)
    end
    contents = File.open(Rails.root.join('public', 'uploads', uploaded_io.original_filename), 'rb').read
    begin
      IJE::MortalityFormat.read(contents) { |record| @uploaded_record = record }
      @good_record = IJE::MortalityFormat.new(@test.data.transform_values { |v| v['value'] })
      good = 0
      problems = []
      successes = []
      @test.data.keys.each do |k|
        if @uploaded_record.send(k) == @good_record.send(k)
          good = good + 1
          found_val = @uploaded_record.send(k)
          found_val = 'nothing' if found_val.blank?
          successes << {
            'summary' => "Found the correct value \"#{found_val}\" for \"#{@test.data[k]['description']}\".",
            'uploaded_value' => found_val,
            'test_value' => @good_record.send(k),
            'description' => @test.data[k]['description'],
            'id' => Digest::MD5.hexdigest(@test.data[k]['description'])
          }
          @test.data[k]['result'] = true
        else
          found_val = @uploaded_record.send(k)
          found_val = 'nothing' if found_val.blank?
          problems << {
            'summary' => "Expected \"#{@good_record.send(k)}\" but found \"#{found_val}\" for \"#{@test.data[k]['description']}\".",
            'uploaded_value' => found_val,
            'test_value' => @good_record.send(k),
            'description' => @test.data[k]['description'],
            'id' => Digest::MD5.hexdigest(@test.data[k]['description'])
          }
          @test.data[k]['result'] = false
        end
      end
      @test.problems = {'problems': problems}
      @test.successes = {'successes': successes}
      @test.complete = true
      @test.score = ((good.to_f / @test.data.keys.count.to_f) * 100).to_i
    rescue Exception => e
      @test.complete = true
      @test.score = 0
      @test.problems = {'problems': [e.to_s]}
    end
    @test.save
    redirect_to test_path @test, system_id: params['system_id'], test_type: params['test_type']
  end

  private

  def new_test
    edrs = current_user.created_systems.find_by(id: params[:system_id])
    case params['test_type']
    when 'ije_export'
      @test = IjeExportTest.create
      @test.populate_fake_data
      edrs.ije_export_tests << @test
    when 'ije_import'
      @test = IjeImportTest.create
      @test.populate_fake_data
      edrs.ije_import_tests << @test
    when 'fhir_export'
      @test = FhirExportTest.create
      @test.populate_fake_data
      edrs.fhir_export_tests << @test
      @test.good_fhir = FhirDeathRecord::Producer.to_fhir({'contents': @test.fhir_mappings, id: '1234567890', certifier_id: '1234567890'}).to_xml.to_s
      @test.save
    when 'fhir_import'
      @test = FhirImportTest.create
      @test.populate_fake_data
      edrs.fhir_import_tests << @test
    else
      raise 'Unknown test type.'
    end
    edrs.save
  end

  def set_test
    case params['test_type']
    when 'ije_export'
      @test = current_user.created_systems.find_by(id: params[:system_id]).ije_export_tests.find_by(id: params[:id])
    when 'ije_import'
      @test = current_user.created_systems.find_by(id: params[:system_id]).ije_import_tests.find_by(id: params[:id])
    when 'fhir_export'
      @test = current_user.created_systems.find_by(id: params[:system_id]).fhir_export_tests.find_by(id: params[:id])
    when 'fhir_import'
      @test = current_user.created_systems.find_by(id: params[:system_id]).fhir_import_tests.find_by(id: params[:id])
    else
      raise 'Unknown test type.'
    end
  end

  def set_tests
    case params['test_type']
    when 'ije_export'
      @tests = current_user.created_systems.find_by(id: params[:system_id]).ije_export_tests
    when 'ije_import'
      @tests = current_user.created_systems.find_by(id: params[:system_id]).ije_import_tests
    when 'fhir_export'
      @tests = current_user.created_systems.find_by(id: params[:system_id]).fhir_export_tests
    when 'fhir_import'
      @tests = current_user.created_systems.find_by(id: params[:system_id]).fhir_import_tests
    else
      raise 'Unknown test type.'
    end
  end
end

require 'rest-client'

class TestController < ApplicationController
  before_action :new_test, only: [:new]
  before_action :set_test, only: [:show, :upload, :download_ije, :submit_checks, :send_fhir]
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
    fhir = FhirProducerHelper.to_fhir(@test.data, @test.id)

    # Post FHIR to given endpoint
    # TODO: Assuming certain param structure (for Nightingale); also assuming JSON!
    RestClient.post target, {fhir: fhir.to_json, email: 'doc1@example.com'}
  end

  def submit_checks
    good = params.keys.select { |k| k.include? 'canary-option' }.count
    @test.complete = true
    @test.score = ((good.to_f / @test.data.keys.count.to_f) * 100).to_i
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
          successes.push("Found the correct value \"#{@uploaded_record.send(k)}\" for \"#{@test.data[k]['description']}\".")
          @test.data[k]['result'] = true
        else
          problems.push("Expected \"#{@good_record.send(k)}\" but found \"#{@uploaded_record.send(k)}\" for \"#{@test.data[k]['description']}\".")
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
    when 'fhir_import'
      @test = FhirImportTest.create
      @test.populate_fake_data
      edrs.fhir_import_tests << @test
    else
      raise 'Not yet implemented.'
    end
    edrs.save
  end

  def set_test
    case params['test_type']
    when 'ije_export'
      @test = current_user.created_systems.find_by(id: params[:system_id]).ije_export_tests.find_by(id: params[:id])
    when 'ije_import'
      @test = current_user.created_systems.find_by(id: params[:system_id]).ije_import_tests.find_by(id: params[:id])
    when 'fhir_import'
      @test = current_user.created_systems.find_by(id: params[:system_id]).fhir_import_tests.find_by(id: params[:id])
    else
      raise 'Not yet implemented.'
    end
  end

  def set_tests
    case params['test_type']
    when 'ije_export'
      @tests = current_user.created_systems.find_by(id: params[:system_id]).ije_export_tests
    when 'ije_import'
      @tests = current_user.created_systems.find_by(id: params[:system_id]).ije_import_tests
    when 'ije_import'
      @tests = current_user.created_systems.find_by(id: params[:system_id]).fhir_import_tests
    else
      raise 'Not yet implemented.'
    end
  end
end

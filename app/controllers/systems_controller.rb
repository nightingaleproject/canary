class SystemsController < ApplicationController
  before_action :new_system, only: [:new, :create]
  before_action :set_system, only: [:show, :destroy, :ije_export_snapshot, :fhir_export_snapshot, :ije_import_snapshot, :fhir_import_snapshot]
  before_action :set_systems, only: [:index]

  # Show dashboard of all user EDRSes.
  def index
  end

  # Show specific EDRS tests.
  def show
    @recent = (@system.ije_export_tests.limit(10) +
               @system.fhir_export_tests.limit(10) +
               @system.ije_import_tests.limit(10) +
               @system.fhir_import_tests.limit(10)).first(10).sort_by(&:created_at).reverse
  end

  # Create new EDRS tests.
  def new
    redirect_to system_path @system
  end

  def destroy
    @system.destroy
    redirect_to systems_path
  end

  def create
    redirect_to system_path @system
  end

  # Returns a snapshot of the 7 most recent test scores for exporting IJE.
  def ije_export_snapshot
    ije_export_data = {}
    @system.ije_export_tests.limit(7).each do |r|
      ije_export_data.merge!(r.created_at.strftime("%m/%d/%Y %r") => r.score)
    end
    render json: ije_export_data
  end

  # Returns a snapshot of the 7 most recent test scores for exporting FHIR.
  def fhir_export_snapshot
    fhir_export_data = {}
    @system.fhir_export_tests.limit(7).each do |r|
      fhir_export_data.merge!(r.created_at.strftime("%m/%d/%Y %r") => r.score)
    end
    render json: fhir_export_data
  end

  # Returns a snapshot of the 7 most recent test scores for importing IJE.
  def ije_import_snapshot
    ije_import_data = {}
    @system.ije_import_tests.limit(7).each do |r|
      ije_import_data.merge!(r.created_at.strftime("%m/%d/%Y %r") => r.score)
    end
    render json: ije_import_data
  end

  # Returns a snapshot of the 7 most recent test scores for importing FHIR.
  def fhir_import_snapshot
    fhir_import_data = {}
    @system.fhir_import_tests.limit(7).each do |r|
      fhir_import_data.merge!(r.created_at.strftime("%m/%d/%Y %r") => r.score)
    end
    render json: fhir_import_data
  end

  private

  def new_system
    name = params[:name].present? ? params[:name] : 'Untitled'
    notes = params[:notes].present? ? params[:notes] : 'No notes provided.'
    @system = System.create(creator: current_user, name: name, notes: notes)
  end

  def set_system
    @system = current_user.created_systems.find_by(id: params[:id])
  end

  def set_systems
     @systems = current_user.created_systems.page(params[:page]).per(30)
  end
end

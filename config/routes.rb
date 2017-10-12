Rails.application.routes.draw do
  devise_for :users

  resources :systems, only: [:show, :index, :new, :destroy] do
    member do
      get :ije_export_snapshot
      get :fhir_export_snapshot
      get :cda_export_snapshot
      get :ije_import_snapshot
      get :fhir_import_snapshot
      get :cda_import_snapshot
    end
  end

  root to: 'systems#index'
end

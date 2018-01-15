Rails.application.routes.draw do
  devise_for :users

  resources :systems, only: [:show, :index, :new, :destroy, :create] do
    member do
      get :ije_export_snapshot
      get :fhir_export_snapshot
      get :cda_export_snapshot
      get :ije_import_snapshot
      get :fhir_import_snapshot
      get :cda_import_snapshot
    end
  end

  resources :test, only: [:show, :index, :new] do
    member do
      post :upload
      post :submit_checks
    end
  end

  post 'send_fhir', to: "test#send_fhir"
  get 'download_ije', to: "test#download_ije"

  root to: 'systems#index'
end

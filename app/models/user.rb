class User < ApplicationRecord
  devise :database_authenticatable, :registerable, :trackable

  has_many :created_systems, class_name: 'System', foreign_key: 'creator_id'
end

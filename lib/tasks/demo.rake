# Rake tasks for setting up Canary for demo use.
namespace :canary do
  namespace :demo do
    desc %(Handy task to configure the database for demo use.

    $ rake canary:demo:setup)
    task setup: :environment do
      print 'Creating demo users... '
      user = User.create!(email: 'user1@example.com', password: '123456')
      user = User.create!(email: 'user2@example.com', password: '123456')
      user = User.create!(email: 'user3@example.com', password: '123456')
      puts 'Done!'
    end
  end
end

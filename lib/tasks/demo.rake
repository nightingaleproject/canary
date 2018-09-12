# Rake tasks for setting up Canary for demo use.
namespace :canary do
  namespace :demo do
    desc %(Handy task to configure the database for demo use.

    $ rake canary:demo:setup)
    task setup: :environment do
      print 'Creating demo users... '
      user = User.create!(email: 'user@example.com', password: '123456', first_name: 'Example', last_name: 'User')
      puts 'Done!'
    end
  end
end

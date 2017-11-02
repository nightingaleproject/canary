# Rake tasks for setting up Canary for demo use.
namespace :canary do
  namespace :demo do
    desc %(Handy task to configure the database for demo use.

    $ rake canary:demo:setup)
    task setup: :environment do
      print 'Creating demo users... '
      user = User.create!(email: 'user@example.com', password: '123456', first_name: 'Example', last_name: 'User')
      puts 'Done!'

      # print 'Creating demo results... '
      # (1..2).each do |s|
      #   edrs_system = System.create(creator: user, name: "Example EDRS ##{rand(0..9999)}", notes: 'Example notes for this EDRS system.')
      #   (1..20).each do |r|
      #     IjeExportTest.create!(score: rand(0..100), system: edrs_system, created_at: r.days.ago)
      #     FhirExportTest.create!(score: rand(0..100), system: edrs_system, created_at: r.days.ago)
      #     CdaExportTest.create!(score: rand(0..100), system: edrs_system, created_at: r.days.ago)
      #     IjeImportTest.create!(score: rand(0..100), system: edrs_system, created_at: r.days.ago)
      #     FhirImportTest.create!(score: rand(0..100), system: edrs_system, created_at: r.days.ago)
      #     CdaImportTest.create!(score: rand(0..100), system: edrs_system, created_at: r.days.ago)
      #   end
      # end
      # puts 'Done!'
    end
  end
end

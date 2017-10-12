# Rake tasks for setting up Canary for demo use.
namespace :canary do
  namespace :demo do
    desc %(Handy task to configure the database for demo use.

    $ rake canary:demo:setup)
    task setup: :environment do
      print 'Creating demo users... '
      user = User.create!(email: 'user@example.com', password: '123456', first_name: 'Example', last_name: 'User')
      puts 'Done!'

      print 'Creating demo results... '
      (1..20).each do |s|
        edrs_system = System.create(creator: user, name: "Example EDRS ##{rand(0..9999)}", notes: 'Example notes for this EDRS system.')
        (1..20).each do |r|
          IjeExportResult.create!(score: rand(0..100), system: edrs_system, created_at: r.days.ago)
          FhirExportResult.create!(score: rand(0..100), system: edrs_system, created_at: r.days.ago)
          CdaExportResult.create!(score: rand(0..100), system: edrs_system, created_at: r.days.ago)
          IjeImportResult.create!(score: rand(0..100), system: edrs_system, created_at: r.days.ago)
          FhirImportResult.create!(score: rand(0..100), system: edrs_system, created_at: r.days.ago)
          CdaImportResult.create!(score: rand(0..100), system: edrs_system, created_at: r.days.ago)
        end
      end
      puts 'Done!'
    end
  end
end

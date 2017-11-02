class IjeImportTest < ApplicationRecord
  default_scope { order('created_at DESC') }

  belongs_to :system, class_name: 'System'

  def populate_fake_data
    self.data = IjeHelper.generate_fake_data
    self.save

    # temp save ije
    filename = SecureRandom.uuid.to_s + '.MOR'
    self.filename = filename
    self.save
    values = self.data.transform_values { |v| v['value'] }
    record = IJE::MortalityFormat.new(values)
    File.open(Rails.root.join('public', filename), 'w') { |file| file.write(IJE::MortalityFormat.write([record])) }
  end
end

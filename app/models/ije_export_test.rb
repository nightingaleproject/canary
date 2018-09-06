class IjeExportTest < ApplicationRecord
  default_scope { order('created_at DESC') }

  belongs_to :system, class_name: 'System'

  def populate_fake_data
    self.data = DataHelper.generate_fake_data
    self.save

    # temp save ije
    aaa = self.data.transform_values { |v| v['value'] }
    record = IJE::MortalityFormat.new(aaa)
    File.open('test.MOR', 'w') { |file| file.write(IJE::MortalityFormat.write([record])) }
  end
end

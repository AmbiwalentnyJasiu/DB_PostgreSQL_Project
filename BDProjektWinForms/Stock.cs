using Npgsql;

namespace BDProjektWinForms {
    public class Stock {
        public static async Task Insert(object store, object product, string amount, NpgsqlConnection conn) {
            if (store != null && product != null) {
                if (amount != "" && int.TryParse(amount, out var newamount)) {
                    try {
                        var storeId = store.ToString().Split("/")[0];
                        var productId = product.ToString().Split("/")[0];
                        await using var cmd = new NpgsqlCommand(
                            $"INSERT INTO \"ProjektBD\".storeproducts VALUES(" +
                            $"{productId}, {storeId}, {newamount})", conn);
                        await cmd.ExecuteNonQueryAsync();

                        MessageBox.Show("Poprawne dodanie produktu do stanu");
                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                    }
                } else {
                    MessageBox.Show("Proszę podać prawidłową ilość");
                }
            } else {
                MessageBox.Show("Proszę wybrać sklep i produkt");
            }
        }

        public static async Task ShowAll( DataGridView dView, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT storeid, name, availableamount, price FROM \"ProjektBD\".storeproducts_view ORDER BY storeid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                dView.Rows.Clear();
                while (await reader.ReadAsync()) {
                    dView.Rows.Insert(iterator++, new[] {
                        Convert.ToString(reader.GetInt32(0)),
                        reader.GetString(1),
                        reader.GetInt32(2).ToString(),
                        reader.GetDouble(3).ToString("N2")});
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public static async Task ShowOne( DataGridView dView, string id, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT storeid, name, availableamount, price FROM \"ProjektBD\".storeproducts_view " +
                                                        "WHERE storeid=" + id + " " +
                                                        "ORDER BY name", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                dView.Rows.Clear();
                while (await reader.ReadAsync()) {
                    dView.Rows.Insert(iterator++, new[] {
                        Convert.ToString(reader.GetInt32(0)),
                        reader.GetString(1),
                        reader.GetInt32(2).ToString(),
                        reader.GetDouble(3).ToString("N2")});
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public static async Task Update(object store, object product, string amount, NpgsqlConnection conn) {
            if (store != null && product != null) {
                if (amount != "" && int.TryParse(amount, out var newamount)) {
                    try {
                        var storeId = store.ToString().Split("/")[0];
                        var productId = product.ToString().Split("/")[0];
                        await using var cmd = new NpgsqlCommand(
                            $"UPDATE \"ProjektBD\".storeproducts " +
                            $"SET availableamount={newamount} " +
                            $"WHERE storeid={storeId} AND productid={productId}", conn);
                        await cmd.ExecuteNonQueryAsync();

                        MessageBox.Show("Poprawna zmiana iloścu produktu na stanie");
                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                    }
                } else {
                    MessageBox.Show("Proszę podać prawidłową ilość");
                }
            } else {
                MessageBox.Show("Proszę wybrać sklep i produkt");
            }
        }
    }
}

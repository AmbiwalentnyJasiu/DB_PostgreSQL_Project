using Npgsql;
using System.Globalization;

namespace BDProjektWinForms {
    public class OrderList {
        public static async Task Insert(object product, object order, string amount, NpgsqlConnection conn) {
            if (order != null && product != null) {
                if (amount != "" && int.TryParse(amount, out var newamount)) {
                    try {
                        var orderId = order.ToString().Split("/")[0];
                        var productId = product.ToString().Split("/")[0];
                        var productVal = Convert.ToDouble(product.ToString().Split("/")[1]);
                        var orderVal = Convert.ToDouble(order.ToString().Split("/")[4]);
                        await using var cmd = new NpgsqlCommand(
                            $"INSERT INTO \"ProjektBD\".orderedproducts VALUES(" +
                            $"{productId}, {orderId}, {newamount})", conn);
                        await cmd.ExecuteNonQueryAsync();

                        var newRval = orderVal + Convert.ToDouble(amount) * productVal;

                        await using var cmd2 = new NpgsqlCommand($"UPDATE \"ProjektBD\".orders " +
                                                                     $"SET price={newRval.ToString(CultureInfo.InvariantCulture)} " +
                                                                     $"WHERE orderId={orderId}", conn);
                        await cmd2.ExecuteNonQueryAsync();

                        MessageBox.Show("Udane dodanie");
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
                await using var cmd = new NpgsqlCommand("SELECT * FROM \"ProjektBD\".orderedproducts_view ORDER BY orderid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                dView.Rows.Clear();
                while (await reader.ReadAsync()) {
                    dView.Rows.Insert(iterator++, new[] {
                        reader.GetInt32(3).ToString(),
                        reader.GetInt32(0).ToString(),
                        reader.GetString(1),
                        reader.GetInt32(2).ToString()});
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public static async Task ShowOne( DataGridView dView, string id, NpgsqlConnection conn ) {
            try {
                await using var cmd = new NpgsqlCommand("SELECT * FROM \"ProjektBD\".orderedproducts_view " +
                                                        "WHERE storeid=" + id + " " +
                                                        "ORDER BY orderid", conn);
                await using var reader = await cmd.ExecuteReaderAsync();
                var iterator = 0;
                dView.Rows.Clear();
                while (await reader.ReadAsync()) {
                    dView.Rows.Insert(iterator++, new[] {
                        reader.GetInt32(3).ToString(),
                        reader.GetInt32(0).ToString(),
                        reader.GetString(1),
                        reader.GetInt32(2).ToString()});
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

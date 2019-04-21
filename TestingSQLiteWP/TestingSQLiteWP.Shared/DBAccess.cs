using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace TestingSQLiteWP
{
    public class DBAccess
    {
        string _DBNAME = "TestSQLImage03";
        #region DB
        public async Task CheckDbAsync()
        {
            try
            {
                StorageFile sf = await ApplicationData.Current.LocalFolder.GetFileAsync(_DBNAME);
            }
            catch (Exception)
            {
                try {
                    DropTableAsync();
                }
                catch { }
                await CreateDatabaseAsync();
            }
        }

        public async Task CreateDatabaseAsync()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(_DBNAME);
            await conn.CreateTableAsync<Test>();
        }

        public async void DropTableAsync()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(_DBNAME);
            await conn.DropTableAsync<Test>();
        }
        #endregion

        #region Test
        public async void AddTestAsync(Test _Test)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(_DBNAME);
            await conn.InsertAsync(_Test);
        }

        public async Task<MyResult> SaveTestAsync(Test _Test)
        {
            try
            {
                SQLiteAsyncConnection conn = new SQLiteAsyncConnection(_DBNAME);
                var Test = await conn.Table<Test>().Where(x => x.Id == _Test.Id).FirstOrDefaultAsync();
                if (Test != null)
                {
                    Test.Id = _Test.Id;
                    Test.Name = _Test.Name;
                    Test.Path = _Test.Path;
                    if (_Test.image != null)
                        Test.image = _Test.image;
                    await conn.UpdateAsync(Test);
                }
                else
                {
                    await conn.InsertAsync(_Test);
                }

                return new MyResult { IsSuccess = true, Message = "Test saved successfully" };
            }
            catch (Exception ex)
            {
                return new MyResult { IsSuccess = false, Message = ex.Message };
            }
        }

        private async Task AddTestListAsync(List<Test> list)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(_DBNAME);
            await conn.InsertAllAsync(list);
        }

        public async Task<List<Test>> LoadTestsAsync()
        {
            List<Test> list = new List<Test>();
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(_DBNAME);

            var query = conn.Table<Test>();
            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                list.Add(new Test
                {
                    Id = item.Id,
                    Name = item.Name,
                    Path = item.Path,
                    image = item.image
                });
            }
            return list;
        }

        public async Task<List<Test>> SearchTestByUserAsync(int id)
        {
            List<Test> list = new List<Test>();
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(_DBNAME);

            var query = conn.Table<Test>().Where(x => x.Id == id);
            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                list.Add(new Test
                {
                    Id = item.Id,
                    Name = item.Name,
                    Path = item.Path,
                    image = item.image
                });
            }

            #region Other ways to loading
            //var allTests = await conn.QueryAsync<Test>("SELECT * FROM Tests");
            //foreach (var item in allTests)
            //{
            //    list.Add(new Test { Id = item.Id, Subject = item.Subject, Date = item.Date, Tags = item.Tags, Time = item.Time, Latitude = item.Latitude, Longitude = item.Longitude, PhotoPath = item.PhotoPath, Text = item.Text });
            //}

            //var myTests = await conn.QueryAsync<Test>("SELECT Name FROM Tests WHERE Subject = ?", new object[] { "Rome, Italy" });
            //foreach (var item in myTests)
            //{
            //    list.Add(new Test { Id = item.Id, Subject = item.Subject, Date = item.Date, Tags = item.Tags, Time = item.Time, Latitude = item.Latitude, Longitude = item.Longitude, PhotoPath = item.PhotoPath, Text = item.Text });
            //}
            #endregion

            return list;
        }

        public async Task UpdateTestAsync(Test item)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(_DBNAME);
            var Test = await conn.Table<Test>().Where(x => x.Id == item.Id).FirstOrDefaultAsync();
            if (Test != null)
            {
                Test.Name = item.Name;
                Test.Path = item.Path;
                await conn.UpdateAsync(Test);
            }
        }

        public async Task<MyResult> DeleteTestAsync(long id)
        {
            MyResult _MyResult = new MyResult { IsSuccess = false, Message = "" };
            try
            {
                SQLiteAsyncConnection conn = new SQLiteAsyncConnection(_DBNAME);
                var Test = await conn.Table<Test>().Where(x => x.Id == id).FirstOrDefaultAsync();
                if (Test != null)
                {
                    await conn.DeleteAsync(Test);

                    _MyResult.IsSuccess = true;
                    _MyResult.Message = "Test deleted successfully";
                }
                else
                {
                    _MyResult.IsSuccess = false;
                    _MyResult.Message = "Invalid Test selected.";
                }
            }
            catch (Exception ex)
            {
                _MyResult.IsSuccess = false;
                _MyResult.Message = ex.Message;
            }

            return _MyResult;
        }
        #endregion
    }
}

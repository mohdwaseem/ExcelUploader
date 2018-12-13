using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace ExcelUploder
{
    [Serializable]
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public SqlConnection Connect()
        {
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-GRTG11N;Initial Catalog=TestUploader;Integrated Security=True");
            con.Open();
            return con;
        }
        
        public List<JobList> GetData()
        {
            List<JobList> jobs = new List<JobList>();
            string query = "SELECT * FROM NIC";
            SqlCommand cmd = new SqlCommand(query,Connect());
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                jobs.Add(new JobList {
                    JobId= Convert.ToInt32(reader[0].ToString()),
                    ProfId= Convert.ToInt32(reader[1].ToString())
                });
            }
            return jobs;
        }
         
        public List<JobList> Jobs
        {
            get { return (List<JobList>)ViewState["_jobs_"]; }
            set { ViewState["_jobs_"] = value; }
        }
        public List<int> WrongData
        {
            get { return (List<int>)ViewState["_wrong_"]; }
            set { ViewState["_wrong_"] = value; }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            bool hasHeader = true;
            var allJobs = GetData();
            List<JobList> correctJobs = new List<JobList>();
            List<int> excelData = new List<int>();
            List<int> notFound = new List<int>();
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(Server.MapPath(FileUpload1.PostedFile.FileName)))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
               
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                   
                    foreach (var cell in wsRow)
                    {
                        excelData.Add(Convert.ToInt32(cell.Text));
                    }
                }
            }
            //Check List Exist in Database or not
            foreach (var item in excelData.Distinct())
            {
                var title = allJobs.Find(x => x.ProfId == item);
                if (title!=null)
                {
                    correctJobs.Add(new JobList { JobId= title.JobId ,ProfId=title.ProfId});
                }
                else
                {
                    notFound.Add(item);
                }
            }
            Jobs = correctJobs;
            WrongData = notFound;


        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            var jobs = Jobs;
            var wrong = WrongData;

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            var jobs = Jobs;
            var wrong = WrongData;
        }
    }
    [Serializable]
    public class JobList
    {
        public int JobId { get; set; }
        public int ProfId { get; set; }
    }
}
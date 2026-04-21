using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QuickDoc.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;

namespace QuickDoc.Repository
{
    public class ProjectRepository
    {
        private Project _project;
        private readonly string ConnectionString;

        public ProjectRepository()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            ConnectionString = config.GetConnectionString("MyDBConnection");
        }

        public Project GetProject()
        {
            if (_project == null)
            {
                return new Project(null, null);
            }
            else
            {
                return _project;
            }
        }

        public void readFromDatabase(string projectNum, UnitRepository unitRepo)
        {
            _project = null;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@" SELECT PR.ProjectNumber, PR.ProjectDescription,
                                                   PRD.PTitle, PRD.PDocDescription, PRD.PFile
                                                   FROM PROJECT PR
                                                   LEFT JOIN PROJECTDOCUMENT PRD 
                                                   ON PR.ProjectNumber = PRD.ProjectNumber
                                                   WHERE PR.ProjectNumber = @projectNum", con);
                cmd.Parameters.AddWithValue("@projectNum", projectNum);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string ProjectNum = dr["ProjectNumber"] == DBNull.Value ? "" : Convert.ToString(dr["ProjectNumber"]);
                        string description = dr["ProjectDescription"]  == DBNull.Value ? "" : Convert.ToString(dr["ProjectDescription"]);  
                        //PTitle
                        string title = dr["PTitle"] == DBNull.Value ? "" : Convert.ToString(dr["PTitle"]);
                        string fileDescription = dr["PDocDescription"] == DBNull.Value ? "" : Convert.ToString(dr["PDocDescription"]);  
                        string filepath = dr["PFile"] == DBNull.Value ? "" : Convert.ToString(dr["PFile"]);

                        Project project = new Project(ProjectNum, description);
                        project.Units = unitRepo.GetAllUnits();
                        _project = project;

                        if (!(title == "" && fileDescription == "" && filepath == ""))
                        {
                            project.Documents.Add(new Document(title, fileDescription, filepath));
                        }
                    }
                }
            }
        }
    }
}


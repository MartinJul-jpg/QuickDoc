using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QuickDoc.Model;
using System;
using System.Collections.Generic;
using System.Text;

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
            Project newProject = new Project(null,null);
            return newProject = _project;
        }

        public void readFromDatabase(string projectNum, UnitRepository unitRepo)
        {
            
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@" SELECT PR.ProjectNumber, PR.ProjectDescription 
                                                PRD.PTitle, PRD.PDocDescription, PRD.PFile
                                                INNER JOIN PROJECTDOCUMENT PRD
                                                FROM PROJECT PR WHERE ProjectNumber = @projectNum", con);
                cmd.Parameters.AddWithValue("@projectNum", projectNum);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string ProjectNum = (string)dr["ProjectNumber"];
                        string description = (string)dr["ProjectDescription"];
                        //FILE
                        string title = (string)dr["TTitle"];
                        string fileDescription = (string)dr["TDocDescription"];
                        string filepath = (string)dr["TFile"];


                        Project project = new Project(ProjectNum, description);
                        project.Units = unitRepo.GetAllUnits();
                        _project = project;
                        project.Documents.Add(new Document(title, fileDescription, filepath));
                    }
                }
            }
        }
    }
}


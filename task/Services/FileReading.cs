using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using task.Models;

namespace task.Services
{
    public class FileReading
    {
        private string filePath { get; set; }
        private Entities entities { get; set; }
        private Logger logger { get; set; }

        public FileReading(string filePath) {
            this.filePath = filePath;
            entities = new Entities();
            logger = new Logger();
        }

        public Response processFile() {

            Response response = new Response();

            try {

                //validating file Path
                if (!File.Exists(filePath) || string.IsNullOrEmpty(filePath)) {
                    logger.addLog(filePath + " not exist");
                    response.statusCode = HttpStatusCode.BadRequest;
                    response.message = "File not exist";
                    return response;
                }

                //getting file extension
                string fileExtension = Path.GetExtension(filePath);

                //validating file extension
                if (fileExtension != ".txt")
                {
                    logger.addLog(filePath + " invalid extension");
                    response.statusCode = HttpStatusCode.BadRequest;
                    response.message = "File Extension must be .txt";
                    return response;
                }

                //saving file data
                logger.addLog(filePath + " processing File");
                processFileData();
                logger.addLog(filePath + " File processed");

                response.statusCode = HttpStatusCode.Accepted;
                response.message = "File Processed Successfully";
                return response;
            }
            catch (Exception e) {
                //handling excewptions
                logger.addLog(filePath + "Exception " + e.Message);
                response.statusCode = HttpStatusCode.InternalServerError;
                response.message = "SERVER ERROR : " + e.Message;
                return response;
            }

        }

        private void processFileData() {

            //getting fileName
            string fileName = Path.GetFileName(filePath);

            //getting file creation Date Time
            DateTime fileCreationDateTime = File.GetCreationTime(filePath);

            FILES file = new FILES();
            file.ID = getSequence("file_seq");
            file.NAME = fileName;
            file.DATETIME = fileCreationDateTime.ToString();

            //adding file meta data to db
            entities.FILES.Add(file);
            entities.SaveChanges();
            logger.addLog(filePath + " meta data saved");

            int fileId = file.ID;

            IEnumerable<string> fileContents = File.ReadLines(filePath);

            foreach (string line in fileContents)
            {
                FILE_CONTENT content = new FILE_CONTENT();
                content.ID = getSequence("file_content_seq");
                content.FILE_ID = fileId;
                content.CONTENT = line;

                entities.FILE_CONTENT.Add(content);
            }

            //adding file contents to db
            entities.SaveChanges();
            logger.addLog(filePath + " content saved");

        }

        private int getSequence(string seqName) {
           int id = entities.Database.SqlQuery<int>($"SELECT {seqName}.NEXTVAL FROM dual").FirstOrDefault();
            return id;
        }
    }
}
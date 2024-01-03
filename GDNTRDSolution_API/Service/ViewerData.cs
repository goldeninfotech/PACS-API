using FellowOakDicom;

namespace GDNTRDSolution_API.Service
{
    public class ViewerData
    {
        public static Dictionary<string, dynamic> GetJson(string host, string study_path)
        {
            Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();
            Dictionary<string, dynamic> studies = new Dictionary<string, dynamic>();



            List<dynamic> list_studies = new List<dynamic>();
            List<dynamic> list_series = new List<dynamic>();


            string StudyInstanceUID = "";
            string StudyDate = "";
            string StudyTime = "";
            string PatientName = "";
            string PatientID = "";
            string AccessionNumber = "";
            string PatientAge = "";
            string PatientSex = "";
            int NumInstances = 0;
            string Modality = "";

            if (study_path != null && study_path != "")
            {
                string[] dir_list = Directory.GetDirectories(study_path);
                if (dir_list.Count() > 0)
                {
                    foreach (string dir in dir_list)
                    {
                        if (dir != "." && dir != ".." & dir != "")
                        {
                            Dictionary<string, dynamic> series = new Dictionary<string, dynamic>();
                            List<dynamic> list_instances = new List<dynamic>();


                            string[] file_list = Directory.GetFiles(dir);

                            string SeriesInstanceUID = "";
                            string SeriesNumber = "";
                            dynamic SliceThickness = "";

                            NumInstances = NumInstances + file_list.Count();

                            foreach (string file in file_list)
                            {
                                Dictionary<string, dynamic> meta = new Dictionary<string, dynamic>();
                                Dictionary<string, dynamic> instance = new Dictionary<string, dynamic>();
                                var meta_data = DicomFile.Open(file);

                                if (StudyInstanceUID == "") { StudyInstanceUID = meta_data.Dataset.GetString(DicomTag.StudyInstanceUID); }
                                if (StudyDate == "") { StudyDate = meta_data.Dataset.GetString(DicomTag.StudyDate); }
                                if (StudyTime == "") { StudyTime = meta_data.Dataset.GetString(DicomTag.StudyTime); }
                                if (PatientName == "") { PatientName = meta_data.Dataset.GetString(DicomTag.PatientName); }
                                if (PatientID == "") { PatientID = meta_data.Dataset.GetString(DicomTag.PatientID); }
                                if (AccessionNumber == "") { AccessionNumber = meta_data.Dataset.GetString(DicomTag.AccessionNumber); }
                                if (PatientAge == "") { PatientAge = meta_data.Dataset.GetString(DicomTag.PatientAge); }
                                if (PatientSex == "") { PatientSex = meta_data.Dataset.GetString(DicomTag.PatientSex); }

                                if (SeriesInstanceUID == "") { SeriesInstanceUID = meta_data.Dataset.GetString(DicomTag.SeriesInstanceUID); }
                                if (SeriesNumber == "") { SeriesNumber = meta_data.Dataset.GetString(DicomTag.SeriesNumber); }
                                if (Modality == "") { Modality = meta_data.Dataset.GetString(DicomTag.Modality); }
                                if (SliceThickness == "") { SliceThickness = meta_data.Dataset.GetString(DicomTag.SliceThickness); }

                                instance.Add("Columns", meta_data.Dataset.GetString(DicomTag.Columns));
                                instance.Add("Rows", meta_data.Dataset.GetString(DicomTag.Rows));
                                instance.Add("InstanceNumber", meta_data.Dataset.GetString(DicomTag.InstanceNumber));
                                instance.Add("SOPClassUID", meta_data.Dataset.GetString(DicomTag.SOPClassUID));
                                instance.Add("PhotometricInterpretation", meta_data.Dataset.GetString(DicomTag.PhotometricInterpretation));
                                instance.Add("BitsAllocated", meta_data.Dataset.GetString(DicomTag.BitsAllocated));
                                instance.Add("BitsStored", meta_data.Dataset.GetString(DicomTag.BitsStored));
                                instance.Add("PixelRepresentation", meta_data.Dataset.GetString(DicomTag.PixelRepresentation));
                                instance.Add("SamplesPerPixel", meta_data.Dataset.GetString(DicomTag.SamplesPerPixel));
                                instance.Add("PixelSpacing", meta_data.Dataset.GetString(DicomTag.PixelSpacing));
                                instance.Add("HighBit", meta_data.Dataset.GetString(DicomTag.HighBit));
                                instance.Add("ImageOrientationPatient", meta_data.Dataset.GetString(DicomTag.ImageOrientationPatient));
                                instance.Add("ImagePositionPatient", meta_data.Dataset.GetString(DicomTag.ImagePositionPatient));
                                instance.Add("FrameOfReferenceUID", meta_data.Dataset.GetString(DicomTag.FrameOfReferenceUID));
                                instance.Add("ImageType", meta_data.Dataset.GetString(DicomTag.ImageType));
                                instance.Add("Modality", meta_data.Dataset.GetString(DicomTag.Modality));
                                instance.Add("SOPInstanceUID", meta_data.Dataset.GetString(DicomTag.SOPInstanceUID));
                                instance.Add("SeriesInstanceUID", meta_data.Dataset.GetString(DicomTag.SeriesInstanceUID));
                                instance.Add("StudyInstanceUID", meta_data.Dataset.GetString(DicomTag.StudyInstanceUID));
                                instance.Add("WindowCenter", (meta_data.Dataset.GetString(DicomTag.WindowCenter).Split("\\"))[1]);
                                instance.Add("WindowWidth", (meta_data.Dataset.GetString(DicomTag.WindowWidth).Split("\\"))[1]);
                                instance.Add("SeriesDate", meta_data.Dataset.GetString(DicomTag.SeriesDate));

                                string inst_url = "dicomweb:" + host + "/api/dicomviewer/file/" + StudyInstanceUID + "/" + SeriesInstanceUID + "/" + meta_data.Dataset.GetString(DicomTag.SOPInstanceUID);

                                meta.Add("metadata", instance);
                                meta.Add("url", inst_url);

                                list_instances.Add(meta);
                            }


                            series.Add("SeriesInstanceUID", SeriesInstanceUID);
                            series.Add("SeriesNumber", SeriesNumber);
                            series.Add("Modality", Modality);
                            series.Add("SliceThickness", SliceThickness);
                            series.Add("instances", list_instances);

                            list_series.Add(series);

                        }
                    }


                }
            }

            studies.Add("StudyInstanceUID", StudyInstanceUID);
            studies.Add("StudyDate", StudyDate);
            studies.Add("StudyTime", StudyTime);
            studies.Add("PatientName", PatientName);
            studies.Add("PatientID", PatientID);
            studies.Add("AccessionNumber", AccessionNumber);
            studies.Add("PatientAge", PatientAge);
            studies.Add("PatientSex", PatientSex);
            studies.Add("series", list_series);
            studies.Add("NumInstances", NumInstances);
            studies.Add("Modalities", Modality);

            list_studies.Add(studies);

            data.Add("studies", list_studies);

            return data;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.TRDModels.Models.TRD
{
public class Hospital
{
	public int Id {get;set;}
	public int User_Id {get;set;}
	public string? Name {get;set;}
	public string? Description {get;set;}
	public string? Email {get;set;}
	public int HospitalCategory_Id { get;set;}
	public string? Country {get;set;}
	public string? City {get;set;}
	public string? Full_Address {get;set;}
	public string? Phone {get;set;}
	public int Status {get;set;}
	public string? AddedDate {get;set;}
	public string? AddedBy {get;set;}
	public string? UpdatedDate {get;set;}
	public string? UpdatedBy {get;set;}
    }
}

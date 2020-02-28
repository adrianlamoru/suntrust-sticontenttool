using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Offer
/// </summary>
public class Offer
{
    public string AprimoOfferID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public OfferContent[] ContentItems { get; set; }
    public Data[] OtherData { get; set; }
	public Offer()
	{
	
	}
}
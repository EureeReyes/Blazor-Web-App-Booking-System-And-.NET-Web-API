using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BookingWebApp.Components.Pages;


public class ApiComm
{
    private readonly HttpClient _httpClient;
    //private const string apiUrl = "http://localhost:5122/users"; // API Base URL

    private const string apiUrl_items = "http://localhost:5122/items"; // API Base URL
    private const string apiUrl_purchase = "http://localhost:5122/purchase"; // API Base URL
    private const string apiUrl_categories = "http://localhost:5122/categories"; // API Base URL
    private const string apiUrl_ItemMaster = "http://localhost:5122/itemmaster"; // API Base URL
    private const string apiUrl_Supplier = "http://localhost:5122/suppliers"; // API Base URL
    private const string apiUrl_PurchaseHeader = "http://localhost:5122/purchaseheader";
    private const string apiUrl_PurchaseDetails = "http://localhost:5122/purchasedetails";
    private const string apiUrl_Uoms = "http://localhost:5122/uoms";
    private const string apiUrl_Inv = "http://localhost:5122/inventories";
    private const string apiUrl_bookingDetails = "http://localhost:5122/bookingDetails";
    private const string apiUrl_bookingHeaders = "http://localhost:5122/bookingHeader";
    private const string apiUrl_Release = "http://localhost:5122/releases";
    private const string apiUrl_Return = "http://localhost:5122/returns";
    private const string apiUrl_cities = "https://psgc.gitlab.io/api/cities/";
    private const string apiUrl_ReleaseDetails = "http://localhost:5122/releaseDetails"; // Add this line
    private const string apiUrl_ReturnDetails = "http://localhost:5122/returnDetails"; // Add this line
    public ApiComm(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Fetch users from API
    //public async Task<List<Items>> GetUsersAsync()
    //{
        //return await _httpClient.GetFromJsonAsync<List<Items>>(apiUrl) ?? new List<Items>();
    //}

    // Fetch items list
    public async Task<List<Items>> GetItemsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Items>>(apiUrl_items) ?? new List<Items>();
    }

        public async Task<List<Purchase>> GetPurchaseAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Purchase>>(apiUrl_purchase) ?? new List<Purchase>();
    }
    

    // Add new item via API
    public async Task<bool> AddItemAsync(Items newItem)
    {
        var response = await _httpClient.PostAsJsonAsync(apiUrl_items, newItem);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AddPurchaseAsync(Purchase newPurchase)
    {
        var response = await _httpClient.PostAsJsonAsync(apiUrl_purchase, newPurchase);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeletePurchaseAsync(int POrderNum)
    {
        var response = await _httpClient.DeleteAsync($"{apiUrl_purchase}/{POrderNum}");
        return response.IsSuccessStatusCode;
    }
//-------------------------------------------------------------------------------------------------------------------
    public async Task<bool> DeleteItemAsync(int itemId)
    {
        var response = await _httpClient.DeleteAsync($"{apiUrl_items}/{itemId}");
        return response.IsSuccessStatusCode;
    }

        public async Task<bool> UpdatePurchaseAsync(Purchase updatedPurchase)
    {
        var response = await _httpClient.PutAsJsonAsync($"{apiUrl_purchase}/{updatedPurchase.POrderNum}", updatedPurchase);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateItemAsync(Items updatedItem)
    {
        var response = await _httpClient.PutAsJsonAsync($"{apiUrl_items}/{updatedItem.ItemId}", updatedItem);
        return response.IsSuccessStatusCode;
    }
//-------------------------------------------------------------------------------------------------------------------

        public async Task<List<Category>> GetCatsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Category>>(apiUrl_categories) ?? new List<Category>();
    }

        public async Task<bool> AddCatsAsync(Category newCats)
    {
        var response = await _httpClient.PostAsJsonAsync(apiUrl_categories, newCats);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCatsAsync(int CategoryId)
    {
        var response = await _httpClient.DeleteAsync($"{apiUrl_categories}/{CategoryId}");
        return response.IsSuccessStatusCode;
    }

        public async Task<bool> UpdateCatsAsync(Category updatedCats)
    {
        var response = await _httpClient.PutAsJsonAsync($"{apiUrl_categories}/{updatedCats.CategoryId}", updatedCats);
        return response.IsSuccessStatusCode;
    }

//-------------------------------------------------------------------------------------------------------------------

        public async Task<List<ItemsMaster>> GetItemMastAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ItemsMaster>>(apiUrl_ItemMaster) ?? new List<ItemsMaster>();
    }

        public async Task<bool> AddItemMastAsync(ItemsMaster newItemsMast)
    {
        var response = await _httpClient.PostAsJsonAsync(apiUrl_ItemMaster, newItemsMast);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteItemMastAsync(int ItemId)
    {
        var response = await _httpClient.DeleteAsync($"{apiUrl_ItemMaster}/{ItemId}");
        return response.IsSuccessStatusCode;
    }

        public async Task<bool> UpdateItemMastAsync(ItemsMaster updatedItemMast)
    {
        var response = await _httpClient.PutAsJsonAsync($"{apiUrl_ItemMaster}/{updatedItemMast.ItemCode}", updatedItemMast);
        return response.IsSuccessStatusCode;
    }
//-------------------------------------------------------------------------------------------------------------------
    public async Task<List<Supplier>> GetSuppliersAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Supplier>>(apiUrl_Supplier) ?? new List<Supplier>();
    }

    // Add new item via API
    public async Task<bool> AddSupplierAsync(Supplier newSupplier)
    {
        var response = await _httpClient.PostAsJsonAsync(apiUrl_Supplier, newSupplier);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteSupplierAsync(int SupplierID)
    {
        var response = await _httpClient.DeleteAsync($"{apiUrl_Supplier}/{SupplierID}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateSupplierAsync(Supplier updatedSupplier)
    {
        var response = await _httpClient.PutAsJsonAsync($"{apiUrl_Supplier}/{updatedSupplier.SupplierId}", updatedSupplier);
        return response.IsSuccessStatusCode;
    }
//-------------------------------------------------------------------------------------------------------------------
    public async Task<List<purchaseHeader>> GetPurchaseHeaderAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<purchaseHeader>>(apiUrl_PurchaseHeader) ?? new List<purchaseHeader>();
    }

    // Add new item via API
    public async Task<bool> AddPurchaseHeaderAsync(purchaseHeader newPheader)
    {
        var response = await _httpClient.PostAsJsonAsync(apiUrl_PurchaseHeader, newPheader);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeletePurchaseHeaderAsync(int POrderNum)
    {
        var response = await _httpClient.DeleteAsync($"{apiUrl_PurchaseHeader}/{POrderNum}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdatePurchaseHeaderAsync(purchaseHeader updatedPheader)
    {
        var response = await _httpClient.PutAsJsonAsync($"{apiUrl_PurchaseHeader}/{updatedPheader.POrderNum}", updatedPheader);
        return response.IsSuccessStatusCode;
    }


//-------------------------------------------------------------------------------------------------------------------
    public async Task<List<purchaseDetails>> GetPurchaseDetailsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<purchaseDetails>>(apiUrl_PurchaseDetails) ?? new List<purchaseDetails>();
    }

    // Add new item via API
    public async Task<bool> AddPurchaseDetailsAsync(List<purchaseDetails> details)
    {
        bool success = true;
        foreach(var detail in details)
        {
            var response = await _httpClient.PostAsJsonAsync(apiUrl_PurchaseDetails, detail);
            success &= response.IsSuccessStatusCode;
        }
        return success;
    }

    public async Task<bool> DeletePurchaseDetailsAsync(int pDetailsID)
    {
        var response = await _httpClient.DeleteAsync($"{apiUrl_PurchaseDetails}/{pDetailsID}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdatePurchaseDetailsAsync(purchaseDetails updatedPdetails)
    {
        var response = await _httpClient.PutAsJsonAsync($"{apiUrl_PurchaseDetails}/{updatedPdetails.pDetailsID}", updatedPdetails);
        return response.IsSuccessStatusCode;
    }
//-------------------------------------------------------------------------------------------------------------------


    public async Task<List<Uoms>> GetUoMAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Uoms>>(apiUrl_Uoms) ?? new List<Uoms>();
    }

    // Add new item via API
    public async Task<bool> AddUoMAAsync(Uoms newUoM)
    {
        var response = await _httpClient.PostAsJsonAsync(apiUrl_Uoms, newUoM);
        return response.IsSuccessStatusCode;
    }


    public async Task<bool> DeleteUoMAsync(int UomId)
    {
        var response = await _httpClient.DeleteAsync($"{apiUrl_Uoms}/{UomId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateUoMAsync(Uoms updatedUoM)
    {
        var response = await _httpClient.PutAsJsonAsync($"{apiUrl_Uoms}/{updatedUoM.UoMId}", updatedUoM);
        return response.IsSuccessStatusCode;
    }
//-------------------------------------------------------------------------------------------------------------------
    public async Task<List<Inventories>> GetInventoryAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Inventories>>(apiUrl_Inv) ?? new List<Inventories>();
    }

    // Add new item via API
    public async Task<bool> AddInventoryAsync(Inventories newInv)
    {
        var response = await _httpClient.PostAsJsonAsync(apiUrl_Inv, newInv);
        return response.IsSuccessStatusCode;
    }


    public async Task<bool> DeleteInventorysync(int InventoryID)
    {
        var response = await _httpClient.DeleteAsync($"{apiUrl_Inv}/{InventoryID}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateInventoryAsync(Inventories updatedInv)
    {
        var response = await _httpClient.PutAsJsonAsync($"{apiUrl_Inv}/{updatedInv.InventoryCode}", updatedInv);
        return response.IsSuccessStatusCode;
    }
    public async Task<bool> UpdateInventoryPatchAsync(int inventoryCode, int? reserved, string? bookingState)
{
    var payload = new
    {
        Reserved = reserved,
        BookingState = bookingState
    };

    var response = await _httpClient.PatchAsJsonAsync($"{apiUrl_Inv}/{inventoryCode}", payload);
    string responseContent = await response.Content.ReadAsStringAsync();

    return response.IsSuccessStatusCode;
}
//-------------------------------------------------------------------------------------------------------------------

    public async Task<List<PhCity>> GetCitiesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<PhCity>>(apiUrl_cities) ?? new List<PhCity>();
    }
//-------------------------------------------------------------------------------------------------------------------
    public async Task<List<BookingHeader>> GetBHeadersAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<BookingHeader>>(apiUrl_bookingHeaders) ?? new List<BookingHeader>();
    }
    public async Task<bool> AddBHeadersAsync(BookingHeader newBHeader)
    {
        var response = await _httpClient.PostAsJsonAsync(apiUrl_bookingHeaders, newBHeader);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateBHeaderAsync(int id, BookingHeader updatedBHeader)
    {
        var response = await _httpClient.PutAsJsonAsync($"{apiUrl_bookingHeaders}/{id}", updatedBHeader);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteBHeaderAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{apiUrl_bookingHeaders}/{id}");
        return response.IsSuccessStatusCode;
    }
//-------------------------------------------------------------------------------------------------------------------

    // BookingDetails Requests
    public async Task<List<BookingDetails>> GetBDetailsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<BookingDetails>>(apiUrl_bookingDetails) ?? new List<BookingDetails>();
    }

    public async Task<List<BookingDetails>> LoadBookingDetails(int bHeaderCode)
    {
        return await _httpClient.GetFromJsonAsync<List<BookingDetails>>($"{apiUrl_bookingDetails}/{bHeaderCode}") 
            ?? new List<BookingDetails>();
    }


    public async Task<bool> AddBDetailsAsync(List<BookingDetails> newBDetailsList)
    {
        var response = await _httpClient.PostAsJsonAsync(apiUrl_bookingDetails, newBDetailsList);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateBDetailsAsync(int id, BookingDetails updatedBDetail)
    {
        var response = await _httpClient.PutAsJsonAsync($"{apiUrl_bookingDetails}/{id}", updatedBDetail);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteBDetailsAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{apiUrl_bookingDetails}/{id}");
        return response.IsSuccessStatusCode;
    }
//-------------------------------------------------------------------------------------------------------------------

    public async Task<List<Release>> GetReleasesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Release>>(apiUrl_Release) ?? new List<Release>();
    }

    public async Task<Release?> GetReleaseByBHeaderCodeAsync(int bHeaderCode)
    {
        return await _httpClient.GetFromJsonAsync<Release>($"{apiUrl_Release}/{bHeaderCode}");
    }

    public async Task<bool> AddReleaseAsync(Release newRelease)
    {
        var response = await _httpClient.PostAsJsonAsync(apiUrl_Release, newRelease);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateReleaseAsync(int id, Release updatedRelease)
    {
        var response = await _httpClient.PutAsJsonAsync($"{apiUrl_Release}/{id}", updatedRelease);
        return response.IsSuccessStatusCode;
    }
public async Task<bool> UpdateReleaseStatusAsync(int id, string? status)
{
    var payload = new
    {
        status = status // Ensure property name matches API expectation
    };

    var response = await _httpClient.PatchAsJsonAsync($"{apiUrl_bookingHeaders}/{id}/status", payload);
    string responseContent = await response.Content.ReadAsStringAsync();

    // Log the response content for debugging
    if (!response.IsSuccessStatusCode)
    {
        Console.WriteLine($"Error updating status: {responseContent}");
    }

    return response.IsSuccessStatusCode;
}

    public async Task<bool> DeleteReleaseAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{apiUrl_Release}/{id}");
        return response.IsSuccessStatusCode;
    }

//-------------------------------------------------------------------------------------------------------------------

    public async Task<List<Returns>> GetReturnsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Returns>>(apiUrl_Return) ?? new List<Returns>();
    }

    public async Task<Returns?> GetReturnByBHeaderCodeAsync(int bHeaderCode)
    {
        return await _httpClient.GetFromJsonAsync<Returns>($"{apiUrl_Return}/{bHeaderCode}");
    }

    public async Task<Returns?> AddReturnAndGetObjectAsync(Returns newReturn)
    {
        var response = await _httpClient.PostAsJsonAsync(apiUrl_Return, newReturn);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Returns>();
        }
        return null;
    }

    public async Task<bool> UpdateReturnAsync(int id, Returns updatedReturn)
    {
        var response = await _httpClient.PutAsJsonAsync($"{apiUrl_Return}/{id}", updatedReturn);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteReturnAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{apiUrl_Return}/{id}");
        return response.IsSuccessStatusCode;
    }

//-------------------------------------------------------------------------------------------------------------------

// Get all ReleaseDetails
public async Task<List<ReleaseDetails>> GetReleaseDetailsAsync()
{
    return await _httpClient.GetFromJsonAsync<List<ReleaseDetails>>(apiUrl_ReleaseDetails) ?? new List<ReleaseDetails>();
}

public async Task<List<ReleaseDetails>> GetReleaseDetailsByReleaseIDAsync(int releaseID)
{
    return await _httpClient.GetFromJsonAsync<List<ReleaseDetails>>($"{apiUrl_ReleaseDetails}?releaseID={releaseID}") ?? new List<ReleaseDetails>();
}

// Add new ReleaseDetails
public async Task<bool> AddReleaseDetailsAsync(ReleaseDetails newReleaseDetail)
{
    var response = await _httpClient.PostAsJsonAsync(apiUrl_ReleaseDetails, newReleaseDetail);
    return response.IsSuccessStatusCode;
}

// Update existing ReleaseDetails
public async Task<bool> UpdateReleaseDetailsAsync(int id, ReleaseDetails updatedReleaseDetail)
{
    var response = await _httpClient.PutAsJsonAsync($"{apiUrl_ReleaseDetails}/{id}", updatedReleaseDetail);
    return response.IsSuccessStatusCode;
}

// Delete ReleaseDetails by ID
public async Task<bool> DeleteReleaseDetailsAsync(int id)
{
    var response = await _httpClient.DeleteAsync($"{apiUrl_ReleaseDetails}/{id}");
    return response.IsSuccessStatusCode;
}

public async Task<bool> UpdateReleaseReturnedAsync(int id, int returned)
{
    var payload = new
    {
        returned = returned // Ensure the property name matches the API expectation
    };

    var response = await _httpClient.PatchAsJsonAsync($"{apiUrl_Release}/{id}/returned", payload);
    string responseContent = await response.Content.ReadAsStringAsync();

    // Log the response content for debugging
    if (!response.IsSuccessStatusCode)
    {
        Console.WriteLine($"Error updating 'Returned' column: {responseContent}");
    }

    return response.IsSuccessStatusCode;
}

// Get all ReturnDetails
public async Task<List<ReturnDetails>> GetReturnDetailsAsync()
{
    return await _httpClient.GetFromJsonAsync<List<ReturnDetails>>(apiUrl_ReturnDetails) ?? new List<ReturnDetails>();
}

// Add new ReturnDetails
public async Task<bool> AddReturnDetailsAsync(ReturnDetails returnDetail)
{
    try
    {
        // Log the details before sending the request
        Console.WriteLine($"Sending ReturnDetail: ReturnID={returnDetail.ReturnID}, InventoryCode={returnDetail.InventoryCode}, Quantity={returnDetail.Quantity}, Description={returnDetail.Description}");

        var response = await _httpClient.PostAsJsonAsync(apiUrl_ReturnDetails, returnDetail);
        
        // Log the response status
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"✅ Successfully added ReturnDetail for InventoryCode: {returnDetail.InventoryCode}");
            return true;
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"❌ Failed to add ReturnDetail for InventoryCode: {returnDetail.InventoryCode}. Error: {errorMessage}");
            return false;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Exception occurred while adding ReturnDetail for InventoryCode: {returnDetail.InventoryCode}. Exception: {ex.Message}");
        return false;
    }
}


// Update existing ReturnDetails
public async Task<bool> UpdateReturnDetailsAsync(int id, ReturnDetails updatedReturnDetails)
{
    var response = await _httpClient.PutAsJsonAsync($"{apiUrl_ReturnDetails}/{id}", updatedReturnDetails);
    return response.IsSuccessStatusCode;
}

// Delete ReturnDetails by ID
public async Task<bool> DeleteReturnDetailsAsync(int id)
{
    var response = await _httpClient.DeleteAsync($"{apiUrl_ReturnDetails}/{id}");
    return response.IsSuccessStatusCode;
}

public async Task<bool> UpdateArchiveDateAsync(int id, DateTime archiveDate)
{
    var payload = new
    {
        archiveDate = archiveDate
    };

    var response = await _httpClient.PatchAsJsonAsync($"{apiUrl_bookingHeaders}/{id}/archive-date", payload);
    string responseContent = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
    {
        Console.WriteLine($"Error updating archive date: {responseContent}");
    }

    return response.IsSuccessStatusCode;
}

public async Task<bool> UpdateCancelStatusAsync(int id, int cancelStatus)
{
    var payload = new
    {
        cancelStatus = cancelStatus
    };

    var response = await _httpClient.PatchAsJsonAsync($"{apiUrl_bookingHeaders}/{id}/cancel-status", payload);
    string responseContent = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
    {
        Console.WriteLine($"Error updating cancel status: {responseContent}");
    }

    return response.IsSuccessStatusCode;
}



}


public class Category
{
    [Key]
    public int CategoryId { get; set; }
    
    [Required]
    public string CategoryName { get; set; } = string.Empty;
}

//-------------------------------------------------------------------------------------------------------------------

public class purchaseHeader{
    public int POrderNum { get; set; }
    public int SupplierId { get; set; }
    public DateOnly? AcquisitionDate { get; set; }
}
//-------------------------------------------------------------------------------------------------------------------

public class purchaseDetails{
    public int pDetailsID {get; set;}   
    public int POrderNum { get; set; }
    public int ItemCode { get; set; } 
    public string Description { get; set; } = string.Empty;
    public double PurchasePrice { get; set; } = 0.00;
    public int Quantity { get; set; } = 1;
    public int UomId { get; set; }

    public string SelectedItemDisplay  { get; set; } = string.Empty;
    public string SelectedUoMCode { get; set; } = string.Empty;     
    public bool AddToInventory { get; set; } = false; 
}
//-------------------------------------------------------------------------------------------------------------------

public class Purchase
{
    [Key]
    public int POrderNum { get; set; }
    public int ItemCode { get; set; }
    public int SupplierId { get; set; }
    public string ItemModel { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly? AcquisitionDate { get; set; }
    public double PurchasePrice { get; set; } = 0.0;
    public int Quantity { get; set; } = 1;
    public ItemsMaster? ItemMaster { get; set; }
}
//-------------------------------------------------------------------------------------------------------------------


public class Items
{
    public int ItemId { get; set; }
    [Required(ErrorMessage = "Item name is required.")]
    public int ItemCode { get; set; }
    public string ItemModel { get; set; } = string.Empty;

    [Required(ErrorMessage = "Supplier name is required.")]
    public string SupplierName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Acquisition date is required.")]
    public DateOnly? AcquisitionDate { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public string Status { get; set; } = string.Empty;

    [Required(ErrorMessage = "Purchase price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal? PurchasePrice { get; set; }

    [Required(ErrorMessage = "Location is required.")]
    public string Location { get; set; } = string.Empty;

    [Column(TypeName = "MEDIUMBLOB")]

    [JsonIgnore]
    public byte[]? ItemImage { get; set; }

    [JsonPropertyName("ItemImage")]
    public string? ItemImageBase64
    {
        get => ItemImage != null ? Convert.ToBase64String(ItemImage) : null;
        set => ItemImage = !string.IsNullOrEmpty(value) ? Convert.FromBase64String(value) : null;
    }

    // Navigation property

}
//-------------------------------------------------------------------------------------------------------------------

public class ItemsMaster{

    public int ItemCode { get; set; }

    [Required(ErrorMessage = "Item name is required")]
    public string ItemName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Item description is required")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "UoM is required")]
    public int UoMId { get; set; } // Nullable to trigger validation

    public string DisplayText => $"ITM{ItemCode} - {Description}";
}
//-------------------------------------------------------------------------------------------------------------------

public class Supplier{
    [Key]
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public string SupplierAddress { get; set; } = string.Empty;
    public string SupplierContactNum { get; set; } = string.Empty;
    public string SupplierEmail { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;

    public string DisplayText => $"SUP{SupplierId} - {SupplierName}";
}
//-------------------------------------------------------------------------------------------------------------------

public class Employee1
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Designation { get; set; }
    public DateOnly DOJ { get; set; }
    public bool IsActive { get; set; }
}
//-------------------------------------------------------------------------------------------------------------------

public class Uoms{
    [Key]
    public int UoMId { get; set; }
    public string UoMCode { get; set; } = string.Empty;
    public string UoMName { get; set; } = string.Empty;
    public int BaseQuantity { get; set; }
}
//-------------------------------------------------------------------------------------------------------------------

public class Inventories
{
    [Key]
    public int InventoryCode { get; set; }
    public int ItemCode { get; set; }
    public int POrderNum { get; set; }
    public int UoMId { get; set; }
    public string? Description { get; set; }  
    public string? Status { get; set; }  
    public int Quantity { get; set; }

    public DateTime? Availability { get; set; }
    public int Reserved { get; set; }
    public string BookingState { get; set; } = string.Empty;


    public string DisplayText => $"INV{InventoryCode} - {Description}";
    public string DisplayInvCode => $"INV{InventoryCode}";

}
//-------------------------------------------------------------------------------------------------------------------

public class PhCity
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
//-------------------------------------------------------------------------------------------------------------------

public class BookingHeader
{
    [Key]
    public int BHeaderCode { get; set; }
    public string? Customer { get; set; }


    [Required(ErrorMessage = "Date Of Booking is required.")]
    public DateOnly? DateOfBooking { get; set; }
    public DateTime? DateofActivity { get; set; }
    public DateTime? EndofActivity { get; set; }


    [Required(ErrorMessage = "Activity Date is required.")]
    public DateOnly? ActivityDate { get; set; }
    public TimeOnly? ActivityTime { get; set; }


    [Required(ErrorMessage = "End Date is required.")]
    public DateOnly? EndDate { get; set; }
    public TimeOnly? EndTime { get; set; }

    public string? Status { get; set; }  
    public string? City { get; set; }  
    public string? Address { get; set; }  
    public DateTime? ArchiveDate { get; set; }
    public int CancelStatus { get; set; } // 0 = Not Cancelled (defualt), 1 = Cancelled

}
//-------------------------------------------------------------------------------------------------------------------

public class BookingDetails
{
    [Key]
    public int BDetailsCode { get; set; }
    public int BHeaderCode { get; set; }
    public int InventoryCode { get; set; }
    public string? Description { get; set; }  
    public int Quantity { get; set; }
    public string? Remarks { get; set; }

    public string SelectedInventoryDisplay  { get; set; } = string.Empty;

}

//-------------------------------------------------------------------------------------------------------------------

public class Release
{
    [Key]
    public int ReleaseID { get; set; }
    public int BHeaderCode { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    [Required(ErrorMessage = "Receiving Person is required.")]
    public string ReceivingPerson { get; set; }

    [Required(ErrorMessage = "Mode of Transport is required.")]
    public string ModeOfTransport { get; set; }

    [Required(ErrorMessage = "Identification Plate is required.")]
    public string IdentificationPlate { get; set; }
    


    public DateOnly? SelectedDate { get; set; } // Stores the date only
    public TimeOnly? SelectedTime { get; set; }
    public DateOnly? SelectedReturnDate { get; set; } // Stores the date only
    public TimeOnly? SelectedReturnTime { get; set; }
}
//-------------------------------------------------------------------------------------------

public class ReleaseDetails
{
    [Key]
    public int ReleaseDetailsID { get; set; } // Primary Key
    public int ReleaseID { get; set; }       // Foreign Key to Release(ReleaseID)
    public int InventoryCode { get; set; }  // Inventory Code
    public string? Description { get; set; } // Description
    public int Quantity { get; set; }       // Quantity
    public string? Remarks { get; set; }

}

//-------------------------------------------------------------------------------------------
public class Returns
{
    [Key]
    public int ReturnID { get; set; }
    public int ReleaseID { get; set; } //FK
    public int BHeaderCode { get; set; } //FK
    public DateTime? ReturnDate { get; set; }
    public DateTime? ActualReturnDate { get; set; }
    public DateTime? ActualReleaseDate { get; set; }
    public string? Remarks { get; set; }

    public DateOnly? SelectedReturnDate { get; set; } // Stores the date only

    public TimeOnly? SelectedReturnTime { get; set; }

    [Required(ErrorMessage = "Actual Return Date is required.")]
    public DateOnly? SelectedActualReturnDate { get; set; } // Stores the date only
    public TimeOnly? SelectedActualReturnTime { get; set; }
    public DateOnly? SelectedActualReleaseDate { get; set; } // Stores the date only

}
//-------------------------------------------------------------------------------------------
public class ReturnDetails
{
    [Key]
    public int ReturnDetailsID { get; set; } 
    public int ReturnID { get; set; } 
    public int ReleaseDetailsID { get; set; } 
    public int InventoryCode { get; set; } 
    public string Description { get; set; } = string.Empty; 
    public int Quantity { get; set; } 
    public string? Remarks { get; set; }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory,
    WebRootPath = "wwwroot",
    EnvironmentName = Environments.Development
});

// SQL CONNECTION -> appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// Service for swagger and endpoints
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS for communication outside localhost
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// TO USE NGINX FOR HEADERS
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

//--------------------------------------------------------------------------------------------

var app = builder.Build();


app.UseForwardedHeaders();


app.UseCors("AllowAll");

// USE SWAGGER
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();


//-----------------------------------Api EndPoints---------------------------------------------------------

// USERS TABLE


app.MapGet("/users", async (MyDbContext dbContext) =>
{
    var users = await dbContext.Users.ToListAsync();
    return Results.Ok(users);
});

app.MapPost("/users", async (MyDbContext dbContext, User user) =>
{
    dbContext.Users.Add(user);
    var result = await dbContext.SaveChangesAsync();
    
    if (result > 0)
    {
        return Results.Created($"/users/{user.UserId}", user);
    }
    return Results.BadRequest("User could not be added.");
});

//--------------------------------------------------------------------------------------------

// ITEMS TABLE

app.MapGet("/Purchase", async (MyDbContext dbContext) =>
{
    var Purchase = await dbContext.Purchase.ToListAsync();
    return Results.Ok(Purchase);
});

app.MapPost("/Purchase", async (MyDbContext dbContext, Purchase Purchase) =>
{
    dbContext.Purchase.Add(Purchase);
    var result = await dbContext.SaveChangesAsync();

    if (result > 0)
    {
        return Results.Created($"/Purchase/{Purchase.POrderNum}", Purchase);
    }
    return Results.BadRequest("Purchase could not be added.");
});

app.MapDelete("/Purchase/{id:int}", async (MyDbContext dbContext, int id) =>
{
    var Purchase = await dbContext.Purchase.FindAsync(id);
    if (Purchase == null)
    {
        return Results.NotFound("Purchase not found.");
    }

    dbContext.Purchase.Remove(Purchase);
    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});

app.MapPut("/Purchase/{id:int}", async (MyDbContext dbContext, int id, Purchase updatedItem) =>
{
    var Purchase = await dbContext.Purchase.FindAsync(id);
    if (Purchase == null)
    {
        return Results.NotFound("Purchase not found.");
    }

    Purchase.ItemModel = updatedItem.ItemModel;
    // Purchase.SupplierName = updatedItem.SupplierName;
    Purchase.AcquisitionDate = updatedItem.AcquisitionDate;
    Purchase.Description = updatedItem.Description;
//    Purchase.Status = updatedItem.Status;
    Purchase.PurchasePrice = updatedItem.PurchasePrice;
    Purchase.Quantity = updatedItem.Quantity;
//    Purchase.Location = updatedItem.Location;
//    Purchase.ItemImage = updatedItem.ItemImage;

    await dbContext.SaveChangesAsync();
    return Results.Ok(Purchase);
});


//-------------------------------------------------------------------------------------------

// Categories TABLE

app.MapGet("/categories", async (MyDbContext dbContext) =>
{
    var categories = await dbContext.Categories.ToListAsync();
    return Results.Ok(categories);
});

app.MapPost("/categories", async (MyDbContext dbContext, Category category) =>
{
    dbContext.Categories.Add(category);
    var result = await dbContext.SaveChangesAsync();

    if (result > 0)
    {
        return Results.Created($"/categories/{category.CategoryId}", category);
    }
    return Results.BadRequest("Category could not be added.");
});

app.MapDelete("/categories/{id:int}", async (MyDbContext dbContext, int id) =>
{
    var category = await dbContext.Categories.FindAsync(id);
    if (category == null)
    {
        return Results.NotFound("Category not found.");
    }

    dbContext.Categories.Remove(category);
    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});

app.MapPut("/categories/{id:int}", async (MyDbContext dbContext, int id, Category updatedCategory) =>
{
    var category = await dbContext.Categories.FindAsync(id);
    if (category == null)
    {
        return Results.NotFound("Category not found.");
    }

    category.CategoryName = updatedCategory.CategoryName;

    await dbContext.SaveChangesAsync();
    return Results.Ok(category);
});


//--------------------------------------------------------------------------------------------

// ITEM MASTER TABLE

app.MapGet("/itemMaster", async (MyDbContext dbContext) =>
{
    var itemsMaster = await dbContext.ItemMaster.ToListAsync();
    return Results.Ok(itemsMaster);
});

app.MapPost("/itemMaster", async (MyDbContext dbContext, ItemsMaster newItemMaster) =>
{
    dbContext.ItemMaster.Add(newItemMaster);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/itemMaster/{newItemMaster.ItemCode}", newItemMaster);
});

app.MapDelete("/itemMaster/{id:int}", async (MyDbContext dbContext, int id) =>
{
    var itemMaster = await dbContext.ItemMaster.FindAsync(id);
    if (itemMaster == null)
    {
        return Results.NotFound("Purchase Master not found.");
    }

    dbContext.ItemMaster.Remove(itemMaster);
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});

app.MapPut("/itemMaster/{id:int}", async (MyDbContext dbContext, int id, ItemsMaster updatedItemMaster) =>
{
    var itemMaster = await dbContext.ItemMaster.FindAsync(id);
    if (itemMaster == null)
    {
        return Results.NotFound("Purchase Master not found.");
    }

    itemMaster.ItemName = updatedItemMaster.ItemName;
    itemMaster.Description = updatedItemMaster.Description;
    itemMaster.UoMId = updatedItemMaster.UoMId;

    await dbContext.SaveChangesAsync();
    return Results.Ok(itemMaster);
});
//-------------------------------------------------------------------------------------------

// SUPPLIERS TABLE

app.MapGet("/suppliers", async (MyDbContext dbContext) =>
{
    var suppliers = await dbContext.Suppliers.ToListAsync();
    return Results.Ok(suppliers);
});

app.MapPost("/suppliers", async (Supplier supplier, MyDbContext dbContext) =>
{
    dbContext.Suppliers.Add(supplier);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/suppliers/{supplier.SupplierId}", supplier);
});

app.MapPut("/suppliers/{id:int}", async (int id, Supplier updatedSupplier, MyDbContext dbContext) =>
{
    var supplier = await dbContext.Suppliers.FindAsync(id);
    if (supplier is null)
    {
        return Results.NotFound();
    }

    supplier.SupplierName = updatedSupplier.SupplierName;
    supplier.SupplierAddress = updatedSupplier.SupplierAddress;
    supplier.SupplierContactNum = updatedSupplier.SupplierContactNum;
    supplier.SupplierEmail = updatedSupplier.SupplierEmail;
    supplier.Industry = updatedSupplier.Industry;

    await dbContext.SaveChangesAsync();
    return Results.Ok(supplier);
});

app.MapDelete("/suppliers/{id:int}", async (int id, MyDbContext dbContext) =>
{
    var supplier = await dbContext.Suppliers.FindAsync(id);
    if (supplier is null)
    {
        return Results.NotFound();
    }

    dbContext.Suppliers.Remove(supplier);
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});
//-------------------------------------------------------------------------------------------
//Purchase Header

app.MapGet("/purchaseHeader", async (MyDbContext db) =>
    await db.PurchaseHeader.ToListAsync());


app.MapPost("/purchaseHeader", async (purchaseHeader ph, MyDbContext db) =>
{
    db.PurchaseHeader.Add(ph);
    await db.SaveChangesAsync();
    return Results.Created($"/purchaseHeaders/{ph.POrderNum}", ph);
});


app.MapPut("/purchaseHeader/{id:int}", async (int id, purchaseHeader input, MyDbContext db) =>
{
    var ph = await db.PurchaseHeader.FindAsync(id);
    if (ph is null) return Results.NotFound();

    // Update properties
    ph.SupplierId = input.SupplierId;
    ph.AcquisitionDate = input.AcquisitionDate;
    await db.SaveChangesAsync();
    return Results.NoContent();
});


app.MapDelete("/purchaseHeader/{id:int}", async (int id, MyDbContext db) =>
{
    var ph = await db.PurchaseHeader.FindAsync(id);
    if (ph is null) return Results.NotFound();

    db.PurchaseHeader.Remove(ph);
    await db.SaveChangesAsync();
    return Results.NoContent();
});
//-------------------------------------------------------------------------------------------
//purhcase Details
app.MapGet("/purchaseDetails", async (MyDbContext db) =>
    await db.PurchaseDetails.ToListAsync());



app.MapPost("/purchaseDetails", async (purchaseDetails pd, MyDbContext db) =>
{
    db.PurchaseDetails.Add(pd);
    await db.SaveChangesAsync();
    return Results.Created($"/purchaseDetails/{pd.pDetailsID}", pd);
});

app.MapPut("/purchaseDetails/{id:int}", async (int id, purchaseDetails input, MyDbContext db) =>
{
    var pd = await db.PurchaseDetails.FindAsync(id);
    if (pd is null) return Results.NotFound();

    // Update properties
    pd.POrderNum = input.POrderNum;
    pd.ItemCode = input.ItemCode;
    pd.Description = input.Description;
    pd.PurchasePrice = input.PurchasePrice;
    pd.Quantity = input.Quantity;
    pd.UoMId = input.UoMId;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/purchaseDetails/{id:int}", async (int id, MyDbContext db) =>
{
    var pd = await db.PurchaseDetails.FindAsync(id);
    if (pd is null) return Results.NotFound();

    db.PurchaseDetails.Remove(pd);
    await db.SaveChangesAsync();
    return Results.NoContent();
});


//-------------------------------------------------------------------------------------------
    // UoMId of Measurement

    
app.MapGet("/uoms", async (MyDbContext db) =>
    await db.UoM.ToListAsync());


app.MapPost("/uoms", async (Uoms uom, MyDbContext db) =>
{
    db.UoM.Add(uom);
    await db.SaveChangesAsync();
    return Results.Created($"/uoms/{uom.UoMId}", uom);
});

app.MapPut("/uoms/{id:int}", async (int id, Uoms updatedUom, MyDbContext db) =>
{
    var uom = await db.UoM.FindAsync(id);
    if (uom is null) return Results.NotFound();

    uom.UoMCode = updatedUom.UoMCode;
    uom.UoMName = updatedUom.UoMName;
    uom.BaseQuantity = updatedUom.BaseQuantity;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/uoms/{id:int}", async (int id, MyDbContext db) =>
{
    var uom = await db.UoM.FindAsync(id);
    if (uom is null) return Results.NotFound();

    db.UoM.Remove(uom);
    await db.SaveChangesAsync();
    return Results.NoContent();
});


app.MapGet("/inventories", async (MyDbContext db) =>
    await db.Inventories.ToListAsync());

app.MapPost("/inventories", async (Inventory inventory, MyDbContext db) =>
{
    db.Inventories.Add(inventory);
    await db.SaveChangesAsync();
    return Results.Created($"/inventories/{inventory.InventoryCode}", inventory);
});

app.MapPut("/inventories/{id:int}", async (int id, Inventory updatedInventory, MyDbContext db) =>
{
    var existingInventory = await db.Inventories.FindAsync(id);
    if (existingInventory is null)
        return Results.NotFound();

    existingInventory.ItemCode = updatedInventory.ItemCode;
    existingInventory.POrderNum = updatedInventory.POrderNum;
    existingInventory.UoMId = updatedInventory.UoMId;
    existingInventory.Description = updatedInventory.Description;
    existingInventory.Status = updatedInventory.Status;
    existingInventory.Quantity = updatedInventory.Quantity;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapPatch("/inventories/{id:int}", async (int id, JsonElement updateData, MyDbContext db) =>
{
    var inventory = await db.Inventories.FindAsync(id);
    if (inventory is null)
        return Results.NotFound();

    if (updateData.TryGetProperty("reserved", out var reservedValue) && reservedValue.ValueKind == JsonValueKind.Number)
    {
        inventory.Reserved = reservedValue.GetInt32();
    }

    if (updateData.TryGetProperty("bookingState", out var bookingStateValue) && bookingStateValue.ValueKind == JsonValueKind.String)
    {
        inventory.BookingState = bookingStateValue.GetString()?.Trim() ?? string.Empty;
    }

    await db.SaveChangesAsync();
    return Results.NoContent();
});



app.MapDelete("/inventories/{id:int}", async (int id, MyDbContext db) =>
{
    var inventory = await db.Inventories.FindAsync(id);
    if (inventory is null)
        return Results.NotFound();

    db.Inventories.Remove(inventory);
    await db.SaveChangesAsync();
    return Results.NoContent();
});


//-------------------------------------------------------------------------------------------

app.MapGet("/bookingheader", async (MyDbContext db) =>
    Results.Ok(await db.BookingHeader.ToListAsync()));

app.MapPost("/bookingheader", async (BookingHeader booking, MyDbContext db) =>
{
    db.BookingHeader.Add(booking);
    await db.SaveChangesAsync();
    return Results.Created($"/bookingheader/{booking.BHeaderCode}", booking);
});

app.MapPut("/bookingheader/{id}", async (int id, BookingHeader updatedBooking, MyDbContext db) =>
{
    var existing = await db.BookingHeader.FindAsync(id);
    if (existing == null) return Results.NotFound();

    existing.Customer = updatedBooking.Customer;
    existing.DateOfBooking = updatedBooking.DateOfBooking;
    existing.DateofActivity = updatedBooking.DateofActivity;
    existing.EndofActivity = updatedBooking.EndofActivity;
    existing.Status = updatedBooking.Status;
    existing.City = updatedBooking.City;
    existing.Address = updatedBooking.Address;
    existing.ArchiveDate = updatedBooking.ArchiveDate;
    existing.CancelStatus = updatedBooking.CancelStatus;

    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.MapPatch("/bookingheader/{id}/status", async (int id, JsonElement updateData, MyDbContext db) =>
{
    var booking = await db.BookingHeader.FindAsync(id);
    if (booking == null) return Results.NotFound($"BookingHeader with ID {id} not found.");

    if (updateData.TryGetProperty("status", out var statusValue) && statusValue.ValueKind == JsonValueKind.String)
    {
        var newStatus = statusValue.GetString()?.Trim();
        if (!string.IsNullOrEmpty(newStatus))
        {
            booking.Status = newStatus;
            await db.SaveChangesAsync();
            return Results.Ok(booking);
        }
    }

    return Results.BadRequest("Invalid status value.");
});

app.MapPatch("/bookingheader/{id}/archive-date", async (int id, JsonElement updateData, MyDbContext db) =>
{
    var booking = await db.BookingHeader.FindAsync(id);
    if (booking == null)
        return Results.NotFound($"BookingHeader with ID {id} not found.");

    if (updateData.TryGetProperty("archiveDate", out var archiveDateValue) &&
        archiveDateValue.ValueKind == JsonValueKind.String &&
        DateTime.TryParse(archiveDateValue.GetString(), out var parsedDate))
    {
        booking.ArchiveDate = parsedDate;
        await db.SaveChangesAsync();
        return Results.Ok(booking);
    }

    return Results.BadRequest("Invalid or missing archiveDate.");
});

app.MapPatch("/bookingheader/{id}/cancel-status", async (int id, JsonElement updateData, MyDbContext db) =>
{
    var booking = await db.BookingHeader.FindAsync(id);
    if (booking == null)
        return Results.NotFound($"BookingHeader with ID {id} not found.");

    if (updateData.TryGetProperty("cancelStatus", out var cancelStatusValue) &&
        cancelStatusValue.TryGetInt32(out var cancelStatus))
    {
        booking.CancelStatus = cancelStatus;
        await db.SaveChangesAsync();
        return Results.Ok(booking);
    }

    return Results.BadRequest("Invalid or missing cancelStatus.");
});




app.MapDelete("/bookingheader/{id}", async (int id, MyDbContext db) =>
{
    var booking = await db.BookingHeader.FindAsync(id);
    if (booking == null) return Results.NotFound();

    db.BookingHeader.Remove(booking);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

//-------------------------------------------------------------------------------------------

app.MapGet("/bookingdetails", async (MyDbContext db) =>
    Results.Ok(await db.BookingDetails.ToListAsync()));

app.MapGet("/bookingdetails/{bHeaderCode}", async (int bHeaderCode, MyDbContext db) =>
{
    Console.WriteLine($"Received bHeaderCode: {bHeaderCode}");

    var details = await db.BookingDetails
        .Where(d => d.BHeaderCode == bHeaderCode)
        .ToListAsync();

    Console.WriteLine($"Fetched {details.Count} booking details");

    return Results.Ok(details);
});

app.MapPost("/bookingdetails", async (List<BookingDetails> details, MyDbContext db) =>
{
    if (details == null || details.Count == 0)
    {
        return Results.BadRequest("No booking details provided.");
    }
    
    db.BookingDetails.AddRange(details);
    await db.SaveChangesAsync();
    return Results.Created($"/bookingdetails", details);
});


app.MapPut("/bookingdetails/{id}", async (int id, BookingDetails updatedDetails, MyDbContext db) =>
{
    var existing = await db.BookingDetails.FindAsync(id);
    if (existing == null) return Results.NotFound();

    existing.BHeaderCode = updatedDetails.BHeaderCode;
    existing.InventoryCode = updatedDetails.InventoryCode;
    existing.Description = updatedDetails.Description;
    existing.Quantity = updatedDetails.Quantity;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/bookingdetails/{id}", async (int id, MyDbContext db) =>
{
    var details = await db.BookingDetails.FindAsync(id);
    if (details == null) return Results.NotFound();

    db.BookingDetails.Remove(details);
    await db.SaveChangesAsync();
    return Results.NoContent();
});


//-------------------------------------------------------------------------------------------


app.MapGet("/releases", async (MyDbContext db) =>
    await db.Releases.ToListAsync());

app.MapGet("/returns/{bHeaderCode:int}", async (int bHeaderCode, MyDbContext db) =>
{
    var returnRecord = await db.Returns.FirstOrDefaultAsync(r => r.BHeaderCode == bHeaderCode);
    if (returnRecord is null) return Results.NotFound();

    return Results.Ok(returnRecord);
});

app.MapPost("/releases", async (MyDbContext db, Release release) =>
{
    db.Releases.Add(release);
    await db.SaveChangesAsync();
    return Results.Created($"/releases/{release.ReleaseID}", release);
});

app.MapPut("/releases/{id:int}", async (MyDbContext db, int id, Release updatedRelease) =>
{
    var existingRelease = await db.Releases.FindAsync(id);
    if (existingRelease is null) return Results.NotFound();

    existingRelease.BHeaderCode = updatedRelease.BHeaderCode;
    existingRelease.ReleaseDate = updatedRelease.ReleaseDate;
    existingRelease.ReturnDate = updatedRelease.ReturnDate;
    existingRelease.ReceivingPerson = updatedRelease.ReceivingPerson;
    existingRelease.ModeOfTransport = updatedRelease.ModeOfTransport;
    existingRelease.IdentificationPlate = updatedRelease.IdentificationPlate;
    existingRelease.Returned = updatedRelease.Returned;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/releases/{id:int}", async (MyDbContext db, int id) =>
{
    var release = await db.Releases.FindAsync(id);
    if (release is null) return Results.NotFound();

    db.Releases.Remove(release);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Add ReleaseDetails endpoints
app.MapGet("/releaseDetails", async (MyDbContext db) =>
    Results.Ok(await db.ReleaseDetails.ToListAsync()));

app.MapPost("/releaseDetails", async (MyDbContext db, ReleaseDetails releaseDetails) =>
{
    db.ReleaseDetails.Add(releaseDetails);
    await db.SaveChangesAsync();
    return Results.Created($"/releaseDetails/{releaseDetails.ReleaseDetailsID}", releaseDetails);
});

app.MapPut("/releaseDetails/{id:int}", async (MyDbContext db, int id, ReleaseDetails updatedReleaseDetails) =>
{
    var existingReleaseDetails = await db.ReleaseDetails.FindAsync(id);
    if (existingReleaseDetails is null) return Results.NotFound();

    // Update properties
    existingReleaseDetails.ReleaseID = updatedReleaseDetails.ReleaseID;
    existingReleaseDetails.InventoryCode = updatedReleaseDetails.InventoryCode;
    existingReleaseDetails.Description = updatedReleaseDetails.Description;
    existingReleaseDetails.Quantity = updatedReleaseDetails.Quantity;

    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.MapPatch("/releases/{id:int}/returned", async (int id, JsonElement updateData, MyDbContext db) =>
{
    var release = await db.Releases.FindAsync(id);
    if (release is null) return Results.NotFound($"Release with ID {id} not found.");

    if (updateData.TryGetProperty("returned", out var returnedValue) && returnedValue.ValueKind == JsonValueKind.Number)
    {
        var newReturnedValue = returnedValue.GetInt32();
        if (newReturnedValue == 0 || newReturnedValue == 1) // Validate the value is 0 or 1
        {
            release.Returned = newReturnedValue;
            await db.SaveChangesAsync();
            return Results.Ok(release);
        }
        return Results.BadRequest("Invalid value for 'returned'. Only 0 or 1 is allowed.");
    }

    return Results.BadRequest("Invalid request body. 'returned' property is required.");
});

app.MapDelete("/releaseDetails/{id:int}", async (MyDbContext db, int id) =>
{
    var releaseDetails = await db.ReleaseDetails.FindAsync(id);
    if (releaseDetails is null) return Results.NotFound();

    db.ReleaseDetails.Remove(releaseDetails);
    await db.SaveChangesAsync();
    return Results.NoContent();
});


//-------------------------------------------------------------------------------------------

// ✅ Get All Returns
app.MapGet("/returns", async (MyDbContext db) =>
    await db.Returns.ToListAsync());

// ✅ Create a New Return (POST)
app.MapPost("/returns", async (MyDbContext db, Return returnRecord) =>
{
    db.Returns.Add(returnRecord);
    await db.SaveChangesAsync();
    return Results.Created($"/returns/{returnRecord.ReturnID}", returnRecord);
});

// ✅ Update an Existing Return (PUT)
app.MapPut("/returns/{id:int}", async (MyDbContext db, int id, Return updatedReturn) =>
{
    var existingReturn = await db.Returns.FindAsync(id);
    if (existingReturn is null) return Results.NotFound();

    existingReturn.BHeaderCode = updatedReturn.BHeaderCode;
    existingReturn.ActualReturnDate = updatedReturn.ActualReturnDate;
    existingReturn.Remarks = updatedReturn.Remarks;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ✅ Delete a Return by ID
app.MapDelete("/returns/{id:int}", async (MyDbContext db, int id) =>
{
    var returnRecord = await db.Returns.FindAsync(id);
    if (returnRecord is null) return Results.NotFound();

    db.Returns.Remove(returnRecord);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Add ReturnDetails endpoints
app.MapGet("/returnDetails", async (MyDbContext db) =>
    Results.Ok(await db.ReturnDetails.ToListAsync()));

app.MapPost("/returnDetails", async (MyDbContext db, ReturnDetails returnDetails) =>
{
    db.ReturnDetails.Add(returnDetails);
    await db.SaveChangesAsync();
    return Results.Created($"/returnDetails/{returnDetails.ReturnDetailsID}", returnDetails);
});

app.MapPut("/returnDetails/{id:int}", async (MyDbContext db, int id, ReturnDetails updatedReturnDetails) =>
{
    var existingReturnDetails = await db.ReturnDetails.FindAsync(id);
    if (existingReturnDetails is null) return Results.NotFound();

    // Update properties
    existingReturnDetails.ReturnID = updatedReturnDetails.ReturnID;
    existingReturnDetails.ReleaseDetailsID = updatedReturnDetails.ReleaseDetailsID;
    existingReturnDetails.InventoryCode = updatedReturnDetails.InventoryCode;
    existingReturnDetails.Description = updatedReturnDetails.Description;
    existingReturnDetails.Quantity = updatedReturnDetails.Quantity;
    existingReturnDetails.Remarks = updatedReturnDetails.Remarks;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/returnDetails/{id:int}", async (MyDbContext db, int id) =>
{
    var returnDetails = await db.ReturnDetails.FindAsync(id);
    if (returnDetails is null) return Results.NotFound();

    db.ReturnDetails.Remove(returnDetails);
    await db.SaveChangesAsync();
    return Results.NoContent();
});


//-------------------------------------------------------------------------------------------
// Force API to listen on all IPs
app.Urls.Add("http://0.0.0.0:5122");

app.Run();

// Database context
public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<Purchase> Purchase { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ItemsMaster> ItemMaster { get; set; }
    public DbSet<Supplier> Suppliers{get; set;}
    public DbSet<purchaseHeader> PurchaseHeader{get; set;}
    public DbSet<purchaseDetails> PurchaseDetails{get; set;}
    public DbSet<Uoms> UoM{get; set;}
    public DbSet<Inventory> Inventories{get; set;}
    public DbSet<BookingHeader> BookingHeader{get; set;}
    public DbSet<BookingDetails> BookingDetails{get; set;}
    public DbSet<Release> Releases { get; set; } 
    public DbSet<ReleaseDetails> ReleaseDetails { get; set; }
    public DbSet<Return> Returns { get; set; } 
    public DbSet<ReturnDetails> ReturnDetails { get; set; } 

}

//----------------------------------Modals for tables---------------------------------------

public class User
{
    public int UserId { get; set; }
    public string? Name { get; set; }
    public DateTime DateofBirth { get; set; }
}
//-------------------------------------------------------------------------------------------
public class Category
{
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
}
//-------------------------------------------------------------------------------------------
// Updated ItemsMaster model
public class ItemsMaster
{
    [Key]
    public int ItemCode { get; set; }

    public string ItemName { get; set; } = string.Empty;

    public string? Description { get; set; }
    public int UoMId { get; set; }
}
//-------------------------------------------------------------------------------------------

public class Supplier{
    [Key]
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public string SupplierAddress { get; set; } = string.Empty;
    public string SupplierContactNum { get; set; } = string.Empty;
    public string SupplierEmail { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;

}

//-------------------------------------------------------------------------------------------
// Purchase model
public class Purchase
{
    [Key]
    public int POrderNum { get; set; }
    public int ItemCode { get; set; }
    public int SupplierId { get; set; }

    public string ItemModel { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly AcquisitionDate { get; set; }
    public double PurchasePrice { get; set; }
    public int Quantity { get; set; }

}
//-------------------------------------------------------------------------------------------

public class purchaseHeader{
    [Key]
    public int POrderNum { get; set; }
    public int SupplierId { get; set; }
    public DateOnly AcquisitionDate { get; set; }
}
//-------------------------------------------------------------------------------------------

public class purchaseDetails{
    [Key]
    public int pDetailsID {get; set;}   
    public int POrderNum { get; set; }
    public int ItemCode { get; set; } 
    public string Description { get; set; } = string.Empty;
    public double PurchasePrice { get; set; } = 1.0;
    public int Quantity { get; set; } = 1;
    public int UoMId { get; set; }
}
//-------------------------------------------------------------------------------------------

public class Uoms{
    [Key]
    public int UoMId { get; set; }
    public string UoMCode { get; set; } = string.Empty;
    public string UoMName { get; set; } = string.Empty;
    public int BaseQuantity { get; set; }
}
//-------------------------------------------------------------------------------------------

public class Inventory
{
    [Key]
    public int InventoryCode { get; set; }  // Matches SQL table
    public int ItemCode { get; set; }
    public int POrderNum { get; set; }
    public int UoMId { get; set; }
    public string? Description { get; set; }  // Nullable in SQL
    public string? Status { get; set; }  // Nullable in SQL
    public int Quantity { get; set; }
    public DateTime? Availability { get; set; }
    public int Reserved { get; set; }
    public string BookingState { get; set; } = string.Empty;



}
//-------------------------------------------------------------------------------------------


public class BookingHeader
{
    [Key]
    public int BHeaderCode { get; set; }
    public string? Customer { get; set; }
    public DateOnly? DateOfBooking { get; set; }
    public DateTime? DateofActivity { get; set; }
    public DateTime? EndofActivity { get; set; }
    public string? Status { get; set; }  
    public string? City { get; set; }  
    public string? Address { get; set; }  
    public DateTime? ArchiveDate { get; set; }
    public int CancelStatus { get; set; }

}
//-------------------------------------------------------------------------------------------

public class BookingDetails
{
    [Key]
    public int BDetailsCode { get; set; }
    public int BHeaderCode { get; set; }
    public int InventoryCode { get; set; }
    public string? Description { get; set; }  
    public int Quantity { get; set; }

}
//-------------------------------------------------------------------------------------------

public class Release
{
    [Key]
    public int ReleaseID { get; set; }
    public int BHeaderCode { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string ReceivingPerson { get; set; } = string.Empty;
    public string ModeOfTransport { get; set; } = string.Empty;
    public string IdentificationPlate { get; set; } = string.Empty;
    public int Returned { get; set; }
}
//-------------------------------------------------------------------------------------------
public class ReleaseDetails
{
    [Key]
    public int ReleaseDetailsID { get; set; }
    public int ReleaseID { get; set; }
    public int InventoryCode { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
}


//-------------------------------------------------------------------------------------------

public class Return
{
    [Key]
    public int ReturnID { get; set; }
    public int ReleaseID { get; set; } //FK
    public int BHeaderCode { get; set; } //FK
    public DateTime ReturnDate { get; set; }
    public DateTime ActualReturnDate { get; set; }
    public DateTime ActualReleaseDate { get; set; }
    public string? Remarks { get; set; }
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
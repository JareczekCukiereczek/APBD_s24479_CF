using Xunit;
using Moq;
using APBD_s24479_CF.Service;
using APBD_s24479_CF.Model;
using APBD_s24479_CF.Context;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using APBD_s24479_CF.DTOModel;

public class PrescriptionsServiceTests
{
    [Fact]
    public void AddPrescription_InvalidDate_ReturnsBadRequest()
    {
        var mockContext = new Mock<ContextEF>();
        var service = new PrescriptionsService(mockContext.Object);
        var prescription = new PrescriptionDTO
        {
            Date = DateTime.Now,
            DueDate = DateTime.Now.AddDays(-1)
        };
        var result = service.AddPrescription(prescription);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Bad date", badRequestResult.Value);
    }

    [Fact]
    public void AddPrescription_TooManyMedicaments_ReturnsBadRequest()
    {
        var mockContext = new Mock<ContextEF>();
        var service = new PrescriptionsService(mockContext.Object);
        var prescription = new PrescriptionDTO
        {
            Date = DateTime.Now,
            DueDate = DateTime.Now.AddDays(1),
            Medicament = new List<MedicamentDTO>(new MedicamentDTO[10])
        };
        var result = service.AddPrescription(prescription);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("ERROR: Too many meds", badRequestResult.Value);
    }

}
using APBD_s24479_CF.Context;
using APBD_s24479_CF.DTOModel;
using APBD_s24479_CF.Model;
using APBD_s24479_CF.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ApbdEfCodeFirst.Tests;

public class PatientsServiceTests
{
    [Fact]
    public void GetPatientData_PatientNotFound_ReturnsNotFound()
    {
        // Arrange
        var mockContext = new Mock<ContextEF>();
        var service = new PatientsService(mockContext.Object);
        int patientId = 1;
        
        mockContext.Setup(c => c.Patients.Any(p => p.IdPatient == patientId)).Returns(false);

        // Act
        var result = service.GetPatientData(patientId);
        
        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal($"Patient with ID {patientId} not found.", notFoundResult.Value);
    }
}

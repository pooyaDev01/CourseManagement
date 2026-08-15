using AutoFixture;
using AutoMapper;
using CourseManagement.Application.Interfaces.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseManagement.Application.Tests.Services;
public class CourseServiceTest
{
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    public CourseServiceTest()
    {
        _courseRepositoryMock = new Mock<ICourseRepository>();
    }


}


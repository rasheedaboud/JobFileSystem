using Core.Entities;
using Core.Features.Contacts.Commands;
using Core.Features.JobFiles.Commands;
using FluentAssertions;
using JobFileSystem.Shared.JobFiles;
using NUnit.Framework;
using System.Text;

namespace IntegrationTests;

using static Testing;

public class JobFilesTest : TestBase
{

    public static async Task CreateContact() => await SendAsync(new CreateContactCommand(new()
    {
        Name = "NWR",
        Company = "NWR",
        ContactMethod = "Email",
        Phone = null,
        ContactType = "Client",
        Email = "raboud@shaw.ca",

    }));


    [Test]
    public async Task ShouldCreateJobFile()
    {
        var userId = await RunAsDefaultUserAsync();

        await CreateContact();

        var jobFile = new JobFileDto()
        {
            Name = "NG PIPING",
            Description = "Fabricate B31.1 piping for Bird Construction.",
            ContactCompany = "NWR",
            
        };

        var command = new CreateJobFile(jobFile);

        var result = await SendAsync(command);


        var item = await FindAsync<JobFile>(result.Id);

        item.Should().NotBeNull();
        item.ShortDescription.Should().Be(command.JobFile.Name);
        item.Description.Should().Be(command.JobFile.Description);
        item.CreatedBy.Should().Be(userId);
        item.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
        item.LastModifiedBy.Should().BeEmpty();
        item.LastModified.Should().BeNull();

    }
    [Test]
    public async Task ShouldCreateJobFileWithAttachments()
    {
        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes("test")))
        {
            var userId = await RunAsDefaultUserAsync();
            await CreateContact();

            var jobFile = new JobFileDto()
            {
                Name = "NG PIPING",
                Description = "Fabricate B31.1 piping for Bird Construction.",
                ContactCompany = "NWR",
            };

            var command = new CreateJobFile(jobFile);

            var result = await SendAsync(command);


            var item = await FindAsync<JobFile>(result.Id);

            item.Should().NotBeNull();
            item.ShortDescription.Should().Be(command.JobFile.Name);
            item.Description.Should().Be(command.JobFile.Description);
            item.CreatedBy.Should().Be(userId);
            item.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
            item.LastModifiedBy.Should().Be("");
            item.LastModified.Should().BeNull();
        }
    }
    [Test]
    public async Task ShouldUpdateJobFile()
    {
        var userId = await RunAsDefaultUserAsync();
        await CreateContact();
        var jobFile = new JobFileDto()
        {
            Name = "NG PIPING",
            Description = "Fabricate B31.1 piping for Bird Construction.",
            ContactCompany = "NWR",
        };

        var command = new CreateJobFile(jobFile);

        var newJobFile = await SendAsync(command);

        var updatedJob = new JobFileDto()
        {
            Id = newJobFile.Id,
            Number = newJobFile.Number,
            Name = "NG",
            Description = "B31.1 piping for Bird Construction.",
            ContactCompany = "NWR",
        };

        var updated = new UpdateJobFile(updatedJob);


        var result = await SendAsync(updated);

        var item = await FindAsync<JobFile>(result.Id);

        item.Should().NotBeNull();
        item.ShortDescription.Should().Be(updated.Jobfile.Name);
        item.Description.Should().Be(updated.Jobfile.Description);
        item.CreatedBy.Should().Be(userId);
        item.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
        item.LastModifiedBy.Should().Be(userId);
        item.LastModified.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
    }
    [Test]
    public async Task ShouldDeleteJobFile()
    {
        var userId = await RunAsDefaultUserAsync();
        await CreateContact();

        var jobFileDto = new JobFileDto()
        {
            Name = "NG PIPING",
            Description = "Fabricate B31.1 piping for Bird Construction.",
            ContactCompany = "NWR",
        };
        var command = new CreateJobFile(jobFileDto);

        var jobFile = await SendAsync(command);

        var result = await SendAsync(new DeleteJobFile(jobFile.Id));

        result.Should().BeTrue();
        
    }
}
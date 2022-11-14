using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Portalum.Fiscalization.SimplePos.Helper
{
    public static class DockerHelper
    {
        public static async Task CleanupAsync()
        {
            var containersListParameters = new ContainersListParameters
            {
                Limit = 1000,
            };

            using var dockerClient = new DockerClientConfiguration().CreateClient();
            var containers = await dockerClient.Containers.ListContainersAsync(containersListParameters).ConfigureAwait(false);

            var filteredContainers = containers.Where(container => container.Labels.Where(o => o.Key == "company" && o.Value == "portalum").Any());
            foreach (var container in filteredContainers)
            {
                if (container.State == "running")
                {
                    try
                    {
                        await dockerClient.Containers.KillContainerAsync(container.ID, new ContainerKillParameters { }).ConfigureAwait(false);
                    }
                    catch (Exception exception)
                    {
                        //logging
                    }
                }
            }
        }

        public static async Task<bool> StartAsync(string dockerImage)
        {
            var createRequest = new CreateContainerParameters
            {
                Image = dockerImage,
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        {
                            $"5618/tcp",
                            new List<PortBinding>
                            {
                                new PortBinding
                                {
                                    HostPort = "5618"
                                }
                            }
                        }
                    }
                },
                Labels = new Dictionary<string, string>
                {
                    {
                        "company", "portalum"
                    },
                    {
                        "application", "efsta.efr"
                    }
                }
            };

            using var dockerClient = new DockerClientConfiguration().CreateClient();
            var createContainerResponse = await dockerClient.Containers.CreateContainerAsync(createRequest);

            return await dockerClient.Containers.StartContainerAsync(createContainerResponse.ID, null);
        }
    }
}

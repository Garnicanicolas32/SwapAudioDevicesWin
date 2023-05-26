using System;
using System.Linq;
using AudioSwitcher.AudioApi.CoreAudio;
using NAudio.CoreAudioApi;
namespace AudioDeviceSwitcher
{

    using System;
    using NAudio.CoreAudioApi;

    class Program
{
    static void Main(string[] args)
    {
        // Create a new CoreAudioController
        var controller = new CoreAudioController();

        while (true)
        {
            // Get all enabled audio playback devices
            var playbackDevices = controller.GetPlaybackDevices((AudioSwitcher.AudioApi.DeviceState)DeviceState.Active);

            // List all available enabled devices
            Console.WriteLine("Available Enabled Audio Devices:");
            for (int i = 0; i < playbackDevices.Count(); i++)
            {
                var device = playbackDevices.ElementAt(i);
                Console.WriteLine($"{i + 1}. {device.Name}");
            }

            // Prompt user to select a device
            Console.WriteLine("Enter the number of the device you want to switch to (or enter 'q' to quit): ");
            string input = Console.ReadLine();

            if (input.ToLower() == "q")
                break;

            if (!int.TryParse(input, out int deviceNumber) || deviceNumber < 1 || deviceNumber > playbackDevices.Count())
            {
                Console.WriteLine("Invalid input. Please try again.");
                continue;
            }

            // Get the selected device
            var selectedDevice = playbackDevices.ElementAt(deviceNumber - 1);

            // Set the selected device as the default playback device
            controller.DefaultPlaybackDevice = selectedDevice;

            Console.WriteLine($"Successfully switched to {selectedDevice.Name}.");
            Console.WriteLine();
        }
    }
}

}

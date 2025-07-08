import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { OBSWebSocket } from "obs-websocket-js";

@Injectable({
  providedIn: "root",
})
export class RecordingService {
  private obs = new OBSWebSocket();

  constructor(private http: HttpClient) {
    this.connectOBS();
  }

  private async connectOBS() {
    try {
      await this.obs.connect("ws://10.32.1.50:4455", "JTZ9kXrdw24Xr4KM");
      console.log("Connected to OBS WebSocket");
    } catch (error) {
      console.error("Failed to connect to OBS WebSocket:", error);
    }
  }

  async startRecording() {
    try {
      return this.obs
        .call("StartRecord")
        .then((response) => {
          console.log("Recording started:", response);
          alert("Recording Started");
        })
        .catch((error) => {
          console.error("Error starting recording:", error);
          alert("Error Starting Recording ")
        });
    } catch (error) {
      console.error("Error starting recording:", error);
    }
  }

  async stopRecording() {
    try {
      return this.obs
        .call("StopRecord")
        .then((response) => {
          console.log("Recording stopped:", response);
          alert("Recording Stopped");

        })
        .catch((error) => {
          console.error("Error stopping recording:", error);
          alert("Error Stopping Recording")
        });
    } catch (error) {
      console.error("Error stopping recording:", error);
    }
  }

  frameRecording(){
    console.log("Haven't built this function yet");
  }
  // async frameRecording(imagePath: string, duration: number = 10000) {
  //   try {
  //     const sceneName = "YourSceneName"; // Replace with the actual scene name
  //     const sourceName = "FrameSource"; // Name of the source to display PNG

  //     console.log("Displaying frame during recording");

  //     // Set up the PNG as the image for the source in OBS

  //     await this.obs.call('SetInputSettings', {
  //       sourceName: sourceName,
  //       sourceSettings: {
  //         file: imagePath,
  //       },
  //     });
            

  //     // Enable the source (display it)
  //     await this.obs.call("SetSceneItemEnabled", {
  //       sceneName: sceneName,
  //       sceneItemId: await this.getSceneItemId(sceneName, sourceName),
  //       enabled: true,
  //     });

  //     // Hide the image after the given duration (e.g., 10 seconds)
  //     setTimeout(async () => {
  //       await this.obs.call("SetSceneItemEnabled", {
  //         sceneName: sceneName,
  //         sceneItemId: await this.getSceneItemId(sceneName, sourceName),
  //         enabled: false,
  //       });
  //       console.log("Frame disabled after", duration, "ms");
  //     }, duration);

  //   } catch (error) {
  //     console.error("Error triggering frame recording:", error);
  //   }
  // }

  // // Helper method to get scene item ID
  // async getSceneItemId(sceneName: string, sourceName: string): Promise<number> {
  //   try {
  //     const response = await this.obs.call("GetSceneItemList", { sceneName });
  //     const sceneItem = response.sceneItems.find(item => item.sourceName === sourceName);

  //     if (!sceneItem) {
  //       throw new Error(`Source '${sourceName}' not found in scene '${sceneName}'`);
  //     }

  //     return sceneItem.sceneItemId;
  //   } catch (error) {
  //     console.error(`Error getting scene item ID for ${sourceName}:`, error);
  //     throw error;
  //   }
  // }
}

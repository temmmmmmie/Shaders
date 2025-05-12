using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class shaders : MonoBehaviour
{
    CommandBuffer _commandBuffer;
    public float _safeZonePct;
    public Mesh _meshQuad;
    public Material _materialSafeZone;

    private void Start()
    {
        StartCoroutine(ScaleScreenCoroutine());
    }

    private IEnumerator ScaleScreenCoroutine()
    {
        while (true)
        {
            // Wait until rendering is complete
            yield return new WaitForEndOfFrame();

            // Create the commands to grab the screen and draw it on a quad
            if (_commandBuffer == null)
            {
                CreateCommandBuffer();
            }

            // Execute the commands
            Graphics.ExecuteCommandBuffer(_commandBuffer);
        }
    }

    private void CreateCommandBuffer()
    {
        _commandBuffer = new CommandBuffer();
        _commandBuffer.name = "Shrink rendering to safe zone";

        // Grab the screen to a temp render texture
        int screenGrabId = Shader.PropertyToID("_ScreenGrabTempTexture");
        _commandBuffer.GetTemporaryRT(screenGrabId, -1, -1, 0, FilterMode.Bilinear);
        _commandBuffer.Blit(BuiltinRenderTextureType.CameraTarget, screenGrabId);

        // Fill the screen with black
        _commandBuffer.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
        _commandBuffer.ClearRenderTarget(clearDepth: false, clearColor: true, Color.black);

        // Set the quad to be pulled in by the safe zone amount
        float scaleFullScreen = 2f; // The quad is (-0.5, -0.5) to (0.5, 0.5) and viewport space is (-1, -1) to (1, 1), so scaling it by 2 fills the viewport
        float showHalfScreenPct = 1f - _safeZonePct; // Safe zone is the percent of the half-width to pull in (be a black bar)
        float scale = showHalfScreenPct * scaleFullScreen;
        _commandBuffer.SetViewProjectionMatrices(Matrix4x4.Scale(new Vector3(scale, scale, 1f)), Matrix4x4.identity);

        // Draw the screen on the quad
        _commandBuffer.SetGlobalTexture("_ScreenGrabTex", screenGrabId); // Set the SafeZone.shader input parameter
        _commandBuffer.DrawMesh(_meshQuad, Matrix4x4.identity, _materialSafeZone);

        // Release the temp render texture
        _commandBuffer.ReleaseTemporaryRT(screenGrabId);
    }

}

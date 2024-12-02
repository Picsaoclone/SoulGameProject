using System;
using System.IO;
using System.Collections; // Thêm thư viện này để sử dụng IEnumerator
using UnityEngine;
using UnityEngine.Networking;

public class ScaryImageToDesktop : MonoBehaviour
{
    // URL ảnh cần tải (cập nhật lại để đảm bảo đúng với ảnh tải xuống)
    public string scaryImageUrl = "https://drive.google.com/uc?export=download&id=1weTc22uD6QfwwhEAfyaC9PDF3yKMYnbV"; // Đảm bảo URL chính xác

    // URL trang web kinh dị
    public string scaryWebsiteUrl = "https://www.google.com/search?q=jumpscare+hornor+gif";

    // Đường dẫn lưu file (sẽ lưu ảnh trên Desktop)
    private string saveImagePath;

    private void Start()
    {
        // Xác định đường dẫn Desktop của người chơi
        saveImagePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/ScaryImage.png";

        // Thực hiện tải ảnh và mở trang web sau 15 giây
        Invoke("DownloadImageAndOpenWebsite", 15f);
    }

    private void DownloadImageAndOpenWebsite()
    {
        // Tải ảnh về Desktop
        StartCoroutine(DownloadImage());

        // Mở trang web kinh dị
        OpenScaryWebsite();
    }

    private IEnumerator DownloadImage()
    {
        // Tạo yêu cầu tải ảnh từ URL
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(scaryImageUrl))
        {
            Debug.Log("Bắt đầu tải ảnh về Desktop...");

            // Chờ đợi cho đến khi ảnh tải xong
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Tải ảnh thành công!");

                // Lưu ảnh vào Desktop
                SaveImageToDesktop(((DownloadHandlerTexture)request.downloadHandler).texture);
            }
            else
            {
                Debug.LogError("Lỗi khi tải ảnh: " + request.error);
            }
        }
    }

    private void SaveImageToDesktop(Texture2D texture)
    {
        // Mã hóa ảnh thành PNG
        byte[] imageData = texture.EncodeToPNG();

        // Ghi dữ liệu vào file trên Desktop
        File.WriteAllBytes(saveImagePath, imageData);

        Debug.Log("Ảnh đã được lưu tại Desktop: " + saveImagePath);
    }

    private void OpenScaryWebsite()
    {
        // Mở trang web kinh dị
        Debug.Log("Đang mở trang web kinh dị...");
        Application.OpenURL(scaryWebsiteUrl);
    }
}

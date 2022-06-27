import 'dart:convert';
import 'package:http/http.dart' as http;
import '/models/image_info.dart';

class ApiService {
  static Future<ImageInfo?> sendImage(filepath) async {
    var request = http.MultipartRequest(
        'POST',
        Uri.parse(
            'https://imageprocessorcards.azurewebsites.net/api/image-processor'));

    request.files.add(await http.MultipartFile.fromPath('image', filepath));

    http.Response response =
        await http.Response.fromStream(await request.send());

    var jsonData = jsonDecode(response.body);
    ImageInfo? imageInfo = ImageInfo.fromJson(jsonData['data']);
    return imageInfo;
  }

  static Future<List<ImageInfo>> getCardsInfo() async {
    final response = await http.get(
        Uri.parse(
            'https://imageprocessorcards.azurewebsites.net/api/image-processor'),
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
        });

    var jsonData = jsonDecode(response.body);

    List<ImageInfo> imagesInfo = [];

    for (var model in jsonData['data']) {
      ImageInfo imageInfo = ImageInfo.fromJson(model);
      imagesInfo.add(imageInfo);
    }

    return imagesInfo;
  }
}

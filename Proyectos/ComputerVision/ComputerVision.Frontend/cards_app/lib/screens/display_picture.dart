import 'dart:io';
import 'package:cards_app/screens/card_details.dart';
import 'package:flutter/material.dart';
import 'package:cards_app/models/image_info.dart' as model;
import '/services/api_service.dart';

class DisplayPictureScreen extends StatefulWidget {
  final String imagePath;

  const DisplayPictureScreen({Key? key, required this.imagePath})
      : super(key: key);

  @override
  State<DisplayPictureScreen> createState() => _DisplayPictureScreenState();
}

class _DisplayPictureScreenState extends State<DisplayPictureScreen> {
  bool isLoading = false;
  model.ImageInfo? imageData;

  _getImageText() async {
    model.ImageInfo? newData = await ApiService.sendImage(widget.imagePath);
    setState(() {
      imageData = newData;
    });
    Navigator.of(context).push(
      MaterialPageRoute(
        builder: (context) => CardDetailsScreen(
          imageInfo: imageData,
        ),
      ),
    );
    setState(() {
      isLoading = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
          title: Container(
        padding: const EdgeInsets.only(left: 82),
        child: const Text('Cards App'),
      )),
      body: SafeArea(
          child: Center(
              child: isLoading
                  ? const CircularProgressIndicator()
                  : Image.file(File(widget.imagePath)))),
      floatingActionButton: Container(
        padding: const EdgeInsets.only(bottom: 62),
        child: isLoading
            ? null
            : FloatingActionButton(
                onPressed: () async {
                  setState(() {
                    isLoading = true;
                  });
                  await _getImageText();
                },
                child: const Icon(Icons.check),
              ),
      ),
      floatingActionButtonLocation:
          FloatingActionButtonLocation.miniCenterFloat,
    );
  }
}

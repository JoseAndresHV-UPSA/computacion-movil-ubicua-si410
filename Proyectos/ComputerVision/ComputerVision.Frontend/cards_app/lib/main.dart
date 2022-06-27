import 'dart:async';
import 'package:camera/camera.dart';
import 'package:cards_app/screens/home.dart';
import 'package:flutter/material.dart';

Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();

  final cameras = await availableCameras();
  final firstCamera = cameras.first;

  runApp(
    MaterialApp(
      // theme: ThemeData.dark(),
      debugShowCheckedModeBanner: false,
      home: HomeScreen(
        camera: firstCamera,
      ),
    ),
  );
}

import 'package:camera/camera.dart';
import 'package:cards_app/screens/historial.dart';
import 'package:flutter/material.dart';
import 'package:scroll_navigation/scroll_navigation.dart';
import 'take_picture.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({Key? key, required this.camera}) : super(key: key);

  final CameraDescription camera;

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  @override
  Widget build(BuildContext context) {
    return ScrollNavigation(
      barStyle: const NavigationBarStyle(
        background: Colors.white,
        elevation: 0.0,
      ),
      pages: [
        TakePictureScreen(camera: widget.camera),
        const HistorialScreen()
      ],
      items: const [
        ScrollNavigationItem(icon: Icon(Icons.camera)),
        ScrollNavigationItem(icon: Icon(Icons.access_time)),
      ],
    );
  }
}

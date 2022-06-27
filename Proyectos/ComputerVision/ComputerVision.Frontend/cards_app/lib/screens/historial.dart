import 'package:cards_app/services/api_service.dart';
import 'package:cards_app/widgets/cards_list.dart';
import 'package:flutter/material.dart';
import '/models/image_info.dart' as model;

class HistorialScreen extends StatefulWidget {
  const HistorialScreen({Key? key}) : super(key: key);

  @override
  State<HistorialScreen> createState() => _HistorialScreenState();
}

class _HistorialScreenState extends State<HistorialScreen> {
  late Future<List<model.ImageInfo>> imagesInfo;

  @override
  void initState() {
    super.initState();

    imagesInfo = ApiService.getCardsInfo();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        backgroundColor: Colors.white,
        appBar: AppBar(
          title: const Center(child: Text('Cards App')),
        ),
        body: SafeArea(child: CardsListWidget(imagesInfo: imagesInfo)));
  }
}

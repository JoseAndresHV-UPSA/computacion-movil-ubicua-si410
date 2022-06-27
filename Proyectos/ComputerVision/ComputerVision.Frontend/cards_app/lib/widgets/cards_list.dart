import 'package:cards_app/screens/card_details.dart';
import 'package:flutter/material.dart';
import '/models/image_info.dart' as model;

// ignore: must_be_immutable
class CardsListWidget extends StatefulWidget {
  CardsListWidget({Key? key, required this.imagesInfo}) : super(key: key);

  Future<List<model.ImageInfo>>? imagesInfo;

  @override
  State<CardsListWidget> createState() => _CardsListWidgetState();
}

class _CardsListWidgetState extends State<CardsListWidget> {
  @override
  Widget build(BuildContext context) {
    return FutureBuilder<List<model.ImageInfo>>(
        future: widget.imagesInfo,
        builder: (context, AsyncSnapshot snapshot) {
          if (snapshot.hasData) {
            return ListView.builder(
              itemCount: snapshot.data.length,
              itemBuilder: (context, index) {
                return ListTile(
                  title: Text(
                    snapshot.data[index].text[0] +
                        " " +
                        snapshot.data[index].text[1] +
                        " " +
                        snapshot.data[index].text[2],
                    overflow: TextOverflow.ellipsis,
                  ),
                  subtitle: Text(snapshot.data[index].phoneNumbers[0] ?? ''),
                  leading: const Icon(Icons.add_card),
                  trailing: const Icon(Icons.keyboard_arrow_right),
                  onTap: () async {
                    model.ImageInfo imageInfo = model.ImageInfo(
                      text: snapshot.data[index].text,
                      emails: snapshot.data[index].emails,
                      phoneNumbers: snapshot.data[index].phoneNumbers,
                      urls: snapshot.data[index].urls,
                    );
                    await Navigator.of(context).push(
                      MaterialPageRoute(
                        builder: (context) =>
                            CardDetailsScreen(imageInfo: imageInfo),
                      ),
                    );
                  },
                );
              },
            );
          } else if (snapshot.hasError) {
            return Center(child: Text('Ocurrio un error: ${snapshot.error}'));
          } else {
            return const Center(child: CircularProgressIndicator());
          }
        });
  }
}

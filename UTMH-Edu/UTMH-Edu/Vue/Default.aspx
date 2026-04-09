<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="UTMH_Edu.Vue.Default" %>

<!DOCTYPE html>
<html lang="zxx" class="no-js">
<head>

    <!-- Mobile Specific Meta -->
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <!-- Favicon-->
    <link rel="shortcut icon" href="img/fav.png">
    <!-- Author Meta -->
    <meta name="author" content="Colorlib">
    <!-- Meta Description -->
    <meta name="description" content="">
    <!-- Meta Keyword -->
    <meta name="keywords" content="">
    <!-- meta character set -->
    <meta charset="UTF-8">
    <link rel="apple-touch-icon" sizes="76x76" href="../assets/img/logo-ct-dark.png" />
    <link rel="icon" type="image/png" href="../assets/img/logo-ct-dark.png" />
    <!-- Site Title -->
    <title>UTMH | Union Des Techniciens Moderne d'Haiti</title>

    <link href="https://fonts.googleapis.com/css?family=Poppins:100,200,400,300,500,600,700" rel="stylesheet">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">

    <!--
			CSS
			============================================= -->
    <link rel="stylesheet" href="../SiteUtilisateur/css/linearicons.css">
    <style type="text/css">
        .auto-style1 {
            font-size: medium;
        }
    </style>
    <style>
        /* Réduit la largeur des zones cliquables sur les côtés */
        #optionsCarousel .carousel-control-prev,
        #optionsCarousel .carousel-control-next {
            width: 1%; /* diminue la zone latérale (ex: 6%) */
        }

        /* Réduit la taille des icônes par défaut de Bootstrap */
        #optionsCarousel .carousel-control-prev-icon,
        #optionsCarousel .carousel-control-next-icon {
            width: 22px; /* taille de l’icône */
            height: 22px;
            background-size: 100% 100%;
            opacity: 0.9;
        }

        /* Optionnel: apparence plus fine et contraste adapté */
        #optionsCarousel .carousel-control-prev-icon,
        #optionsCarousel .carousel-control-next-icon {
            filter: invert(1); /* flèches blanches si fond sombre */
        }

        #optionsCarousel .carousel-control-prev:hover .carousel-control-prev-icon,
        #optionsCarousel .carousel-control-next:hover .carousel-control-next-icon {
            opacity: 1;
        }

        /* Optionnel: recentrer verticalement les flèches si nécessaire */
        #optionsCarousel .carousel-control-prev,
        #optionsCarousel .carousel-control-next {
            align-items: center; /* centre verticalement l’icône */
        }

        /* Optionnel: réduire la taille des “dots” indicateurs si présents */
        #optionsCarousel .carousel-indicators [data-bs-target] {
            width: 8px;
            height: 8px;
            border-radius: 50%;
        }
    </style>
    <link rel="stylesheet" href="../SiteUtilisateur/css/font-awesome.min.css">
    <link rel="stylesheet" href="../SiteUtilisateur/css/magnific-popup.css">
    <link rel="stylesheet" href="../SiteUtilisateur/css/nice-select.css">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="../SiteUtilisateur/css/bootstrap.css">
    <link rel="stylesheet" href="../SiteUtilisateur/css/main.css">
   <style>
    /* ===== HERO SLIDER ===== */
    .banner-area {
        position: relative;
        overflow: hidden;
        height: 915px;
    }

    .banner-slider {
        position: absolute;
        inset: 0;
        z-index: 0;
    }

    .slide-item {
        position: absolute;
        inset: 0;
        background-size: cover;
        background-position: center;
        background-repeat: no-repeat;
        opacity: 0;
        transition: opacity 1.2s ease-in-out;
    }

    .slide-item.active {
        opacity: 1;
    }

    .banner-area .overlay-bg {
        position: absolute;
        inset: 0;
        z-index: 1;
        background: rgba(0, 0, 0, 0.55);
    }

    /* Texte centré par-dessus */
    .slider-text-wrap {
        position: absolute;
        inset: 0;
        z-index: 2;
        display: flex;
        align-items: center;
        padding-left: 60px;
    }

    .slider-text-inner {
        max-width: 750px;
    }

    .slider-title {
        color: #ffffff;
        font-size: 48px;
        font-weight: 700;
        line-height: 1.2;
        margin-bottom: 20px;
        opacity: 0;
        transform: translateY(30px);
        transition: opacity 0.7s ease 0.3s, transform 0.7s ease 0.3s;
    }

    .slider-subtitle {
        color: #f0f0f0;
        font-size: 22px;
        font-style: italic;
        opacity: 0;
        transform: translateY(20px);
        transition: opacity 0.7s ease 0.5s, transform 0.7s ease 0.5s;
    }

    /* Quand le texte est visible */
    .slider-text-wrap.text-visible .slider-title,
    .slider-text-wrap.text-visible .slider-subtitle {
        opacity: 1;
        transform: translateY(0);
    }

    /* Dots */
    .slider-dots {
        position: absolute;
        bottom: 30px;
        left: 50%;
        transform: translateX(-50%);
        display: flex;
        gap: 10px;
        z-index: 3;
    }

    .slider-dot {
        width: 10px;
        height: 10px;
        border-radius: 50%;
        background: rgba(255, 255, 255, 0.45);
        border: 2px solid rgba(255, 255, 255, 0.8);
        cursor: pointer;
        transition: background 0.3s, transform 0.3s;
        padding: 0;
    }

    .slider-dot.active {
        background: #ffffff;
        transform: scale(1.3);
    }

    /* Barre de progression */
    .slider-progress {
        position: absolute;
        top: 0;
        left: 0;
        height: 3px;
        background: rgba(255, 255, 255, 0.85);
        z-index: 4;
        width: 0%;
        transition: width linear;
    }
</style>
</head>

<body>
    <form id="form1" runat="server">
        <div>
            <!-- Start Header Area -->
            <header class="default-header">
                <div class="container">
                    <div class="header-wrap">
                        <div class="header-top d-flex justify-content-between align-items-center">
                            <div class="logo">
                                <a href="#home">
                                    <img src="../SiteUtilisateur/img/logo1.png" alt=""></a>
                            </div>
                            <div class="main-menubar d-flex align-items-center">
                                <nav class="hide">
                                    <a href="#home" class="auto-style1">Accueil</a>
                                    <a href="#project" class="auto-style1">Options</a>
                                    <a href="#about" class="auto-style1">A propos</a>
                                    <a href="/Vue/FormConnexion.aspx" class="auto-style1">Se connecter</a>

                                    <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">S'inscrire</button>
                                </nav>
                                <div class="menu-bar"><span class="lnr lnr-menu"></span></div>
                            </div>
                        </div>
                    </div>
                </div>
            </header>
            <!-- End Header Area -->

            <!-- Button trigger modal -->


            <!-- Modal pour inscription -->
            <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-lg modal-dialog-scrollable">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="exampleModalLabel">Inscription Etudiant</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">

                            <div class="card-body">
                                <!-- 🔴 FRAIS D'INSCRIPTION -->
                                <div class="card border-danger mb-4 shadow-sm">
                                    <div class="card-body text-center">
                                        <h5 class="text-danger fw-bold mb-2">💳 Frais d'inscription obligatoires
                                        </h5>
                                        <h4 class="fw-bold text-dark">1 500 GDES
                                        </h4>
                                        <p class="mb-0 text-muted">
                                            Le paiement est requis pour finaliser l'inscription.
                                        </p>
                                    </div>
                                </div>

                                <div class="row g-3">

                                    <div class="col-md-6">
                                        <label>Prénom</label>
                                        <asp:TextBox ID="txtPrenom" CssClass="form-control" runat="server" oninput="
                                        this.value = this.value
                                        .replace(/[^a-zA-ZÀ-ÿ _-]/g, '')
                                        .replace(/([ _-])\1+/g, '$1')
                                        .replace(/^([ _-])/, '')
                                        " />
                                    </div>

                                    <div class="col-md-6">
                                        <label>Nom</label>
                                        <asp:TextBox ID="txtNom" CssClass="form-control" runat="server" oninput="
                                        this.value = this.value
                                        .replace(/[^a-zA-ZÀ-ÿ _-]/g, '')
                                        .replace(/([ _-])\1+/g, '$1')
                                        .replace(/^([ _-])/, '')
                                        " />
                                    </div>

                                    <div class="col-md-6">
                                        <label>Date de naissance</label>
                                        <asp:TextBox ID="txtDateNaissance" CssClass="form-control" runat="server" TextMode="Date" />
                                    </div>

                                    <div class="col-md-6">
                                        <label>Sexe</label>
                                        <asp:DropDownList ID="ddlSexe" CssClass="form-control" runat="server">
                                            <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>
                                            <asp:ListItem>Féminin</asp:ListItem>
                                            <asp:ListItem>Masculin</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-md-6">
                                        <label>Email</label>
                                        <asp:TextBox ID="txtMail" CssClass="form-control" runat="server" TextMode="Email" />
                                    </div>

                                    <div class="col-md-6">
                                        <label>Téléphone</label>
                                        <asp:TextBox ID="txtPhone" CssClass="form-control phone"
                                            placeholder="+50938651308" pattern="\+509[0-9]{8}"
                                            MaxLength="12" TextMode="Phone" runat="server" />
                                    </div>

                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Departement</label>
                                            <asp:DropDownList
                                                ID="ddlDepartement"
                                                runat="server"
                                                CssClass="form-control"
                                                AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlDepartement_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Ville</label>
                                            <asp:DropDownList
                                                ID="ddlVille"
                                                runat="server"
                                                CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-md-6">
                                        <label>Adresse</label>
                                        <asp:TextBox ID="txtAdresse" CssClass="form-control" runat="server" />
                                    </div>

                                    <div class="col-md-6">
                                        <label>Telephone Responsable </label>
                                        <asp:TextBox ID="txtTelephoneRes" CssClass="form-control phone" runat="server" TextMode="Phone" />
                                    </div>
                                    <div class="col-md-6">
                                        <label>Personne Responsable</label>
                                        <asp:TextBox ID="txtPersonneRes" CssClass="form-control" runat="server" oninput="
                                        this.value = this.value
                                        .replace(/[^a-zA-ZÀ-ÿ _-]/g, '')
                                        .replace(/([ _-])\1+/g, '$1')
                                        .replace(/^([ _-])/, '')
                                        " />
                                    </div>
                                    <div class="col-md-6">
                                        <label>cin</label>
                                        <asp:TextBox ID="txtCin" CssClass="form-control cin" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6">
                                        <label>Option choisie</label>
                                        <asp:DropDownList ID="ddlOption" CssClass="form-control" runat="server" />
                                    </div>
                                   

                                   <%-- <div class="col-12 text-center mt-4">

                                        <asp:LinkButton
                                            ID="btnMonCash"
                                            runat="server"
                                            CssClass="btn btn-danger btn-lg px-4 d-inline-flex align-items-center justify-content-center"
                                            Visible="false"
                                            OnClick="btnMonCash_Click">

        <img src="../assets/img/moncash.png"
             alt="MonCash"
             style="height:30px; margin-right:10px;" />

        <span>Payer avec MonCash</span>

                                        </asp:LinkButton>

                                    </div>--%>

                                    <asp:Label ID="lblErreur" runat="server" Text=""></asp:Label>


                                    <%-- <label>Date Inscription</label>--%>
                                    <asp:TextBox ID="txtDateInscription" runat="server" ReadOnly="true" Visible="false" />


                                    <%-- <label>Année académique</label>--%>
                                    <asp:DropDownList ID="ddlAnneeAcademique" runat="server" Visible="False" />



                                    <%--  <label>Code</label>--%>
                                    <asp:TextBox ID="txtCode" runat="server" ReadOnly="true" Visible="false" />

                                     <div class="col-md-6">
                                      <%--  <label>Mode de paiement</label>--%>
                                        <asp:DropDownList ID="ddlModePaiement" CssClass="form-control"
                                            runat="server" AutoPostBack="true"
                                            visible="false">
                                            <asp:ListItem Value="">-- Sélectionner --</asp:ListItem>
                                            <asp:ListItem Value="NatCash">NatCash</asp:ListItem>
                                            <asp:ListItem Value="MonCash">MonCash</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>



                                    <%--<label>Photo de profil</label>--%>
                                    <asp:FileUpload ID="fuPhotoProfil" runat="server" Visible="False" />

                                    <%--  <!-- <label for="example-text-input" uuuss="form-control-label">Role</label> -->--%>
                                    <asp:TextBox ID="txtRole" runat="server" Visible="False"></asp:TextBox>
                                </div>

                                <hr />



                            </div>

                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnAnnuler" runat="server" class="btn btn-secondary" data-bs-dismiss="modal" Text="Annuler" OnClick="btnAnnuler_Click" />
                             <asp:Button ID="btnEnvoye" runat="server" class="btn btn-primary" Text="Envoyer" OnClick="btnEnvoyerEtudiant_Click" />
                        </div>
                    </div>
                </div>
            </div>

           <!-- Start banner Area -->
<section class="banner-area relative" id="home">
    <div class="banner-slider">
        <div class="slide-item active"
             style="background-image: url('../SiteUtilisateur/img/slider1.png');"
             data-title="Formons ensemble les techniciens de demain"
             data-subtitle="Un pays, Une École, Notre avenir.">
        </div>
        <div class="slide-item"
             style="background-image: url('../SiteUtilisateur/img/about-img.jpg');"
             data-title="Excellence & Professionnalisme"
             data-subtitle="Des formations adaptées au marché de l'emploi haïtien.">
        </div>
        <div class="slide-item"
             style="background-image: url('../SiteUtilisateur/img/slider1.png');"
             data-title="Rejoignez notre communauté"
             data-subtitle="Plus de 10 options de formation disponibles dès aujourd'hui.">
        </div>
        <div class="slide-item"
             style="background-image: url('../SiteUtilisateur/img/about-img.jpg');"
             data-title="Construisons l'Haïti de demain"
             data-subtitle="La technologie au service du développement national.">
        </div>
    </div>
    <div class="overlay overlay-bg"></div>

    <!-- Texte dynamique par slide -->
    <div class="slider-text-wrap">
        <div class="slider-text-inner">
            <h1 class="slider-title"></h1>
            <h3 class="slider-subtitle"></h3>
        </div>
    </div>

    <!-- Dots de navigation -->
    <div class="slider-dots" id="sliderDots"></div>
</section>
<!-- End banner Area -->
            <!-- Modal -->
            <div class="modal fade" id="exampleLargeModal" tabindex="-1" role="dialog" aria-labelledby="exampleLargeModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-lg  modal-fullscreen" role="document">
                    <div class="modal-content">

                        <div class="modal-header">
                            <h4 class="modal-title" id="exampleLargeModalLabel">UTMH</h4>
                            <button type="button" class="close" data-dismiss="modal">
                                <span>&times;</span>
                            </button>
                        </div>

                        <div class="modal-body">
                            <h5>UTMH:</h5>
                            <p>
                                Union des Techniciens Modernes d’Haïti  UTMH  École Professionnelle a vu le jour le 20 Octobre 2012 à
                             Pétion-Ville. C’est un établissement privé à caractère scientifique, culturel et Technique fonctionnant
                             à but non-lucratif. Elle est située au nemero 40, Delmas 95, Jacquet Tybull Rue  Pomeyrac Prolongée impasse
                             Louis Jeanty #40 au local de l’INSTITUTION MIXTE LOUIS JEANTY.
                            </p>
                            <p>Sa mission est de:</p>
                            <p>1) Former des jeunes et les apprenants aux profits voulus et recherchés par le marché de l’emploi</p>
                            <p>
                                2) Favoriser et accélérer  l’inspection professionnelle des apprenants et des stagiaires en développant, et en améliorant ou en adoptant
     leurs compétences  techniques et professionnelle en les accompagnants dans leurs projets professionnelles en vue de création d’entreprise.
                            </p>
                            4) Cours disponibles
                            <p>❖	Hôtellerie et Tourisme en 18 mois</p>
                            <p>❖	Cosmétologie en 9 mois</p>
                            <p>❖	Cuisine et Pâtisserie en 9 mois</p>
                            <p>
                                ❖	Carrelage en 6 mois/<p>
                            <p>❖	Technique Windows en 6 mois</p>
                            <p>❖	Électricité Bâtiment en 9 mois</p>
                            <p>❖	Plomberie en 9 mois</p>
                            <p>❖	Danse et Musique</p>
                            <p>5) Les cours sont dispensés en Weekend </p>
                            <p>Samedi : 8h-12h /1h-5h</p>
                            <p>Dimanche : 1h-5h</p>
                            <p>6) Politique des cours</p>
                            -
                            <p>GENERALITES </p>
                            :
                            <p>
                                De l’Hôtellerie et Tourisme   : Hébergement – Bar et Restaurant – Cuisine et Pâtisserie - Tourisme . autres cours Comme : Anglais commercial – Espagnole commercial – Français de base – Technologie-Français commerciale – Gestion stock – Informatique –Cours de massage- Comptabilité –Leadership-Législation – Formation à l’emploi ect. 
-Cosmétologie :   GENERALITES :COSMETOLOGIE ou DERMATOLOGIE ESTHETIQUE-CHIRURGIE ESTHETIQUE ou CHIRURGIE CORRECTRICE-Technologie - 
     autres cours comme : leadership – Anglais de base – Français de base –Informatique – Formation en Législation – Formation à l’emploi.
                            </p>
                            <p>Cuisine et Pâtisserie :</p>
                            <p>GENERALITES :</p>
                            <p>
                                Hygiène alimentaire – Intoxication des alimentaire – les différents recettes en cuisine –Technologie- différents type de pates – 
    autre cours comme : Leadership – Anglais de base – Français de base – Informatique – Formation en Législation – Formation à l’emploi.
                            </p>
                            <p>Carrelage :</p>
                            <p>GENERALITES :</p>
                            <p>
                                Cours de Dessin – Cours de Géométrie – Pose céramique – Technologie- autre cours comme : Leadership – Anglais de base – Français de base – Informatique 
                            – Formation en Législation – Formation à l’emploi..
                            </p>

                            <p>Technique Windows :</p>
                            <p>GENERALITES :</p>
                            <p>
                                Cours  de Dessin – Cours de Géométrie – Windows and door – Technologie - autre cours comme : Leadership – Anglais de base – Français de base – Informatique 
    – Formation en Législation – Formation à l’emploi.
                            </p>


                        </div>

                    </div>
                </div>
            </div>


            <!-- End banner Area -->

            <!-- Start callto Area -->
            <section class="callto-area relative">
                <div class="container">
                    <div class="row d-flex callto-wrap justify-content-between pt-40 pb-40">
                        <h2 class="text-white">L’UTMH œuvre pour la formation, l’accompagnement et le développement des techniciens modernes en Haïti.</h2>

                    </div>
                </div>
            </section>
            <!-- End callto Area -->


            <!-- Start project Area -->
            <section class="project-area section-gap" id="project">
                <div class="container">
                    <div class="row d-flex justify-content-center">
                        <div class="col-md-8 pb-80 header-text text-center">
                            <h1>Nos Options</h1>
                            <p>
                                Découvrez nos formations,
          <br>
                                de développement pour les jeunes techniciens haïtiens.
                            </p>
                        </div>
                    </div>

                    <!-- Carousel -->
                    <div id="optionsCarousel" class="carousel slide" data-bs-ride="carousel">
                        <div class="carousel-inner">

                            <!-- Première slide (3 projets) -->
                            <div class="carousel-item active">
                                <div class="row g-4">
                                    <!-- g-4 ajoute de l’espace entre les colonnes -->
                                    <!-- Projet 1 -->
                                    <div class="col-lg-4 col-md-6 project-wrap">
                                        <div class="single-project h-100">
                                            <div class="content">
                                                <a href="#" target="_blank">
                                                    <div class="content-overlay"></div>
                                                    <img class="content-image img-fluid rounded" src="../SiteUtilisateur/img/massage.jpg" alt="">
                                                </a>
                                            </div>
                                            <div class="details mt-3">
                                                <h2 class="text-uppercase">massothérapie</h2>
                                                <p class="read-text">
                                                    La massothérapie est une thérapie manuelle qui utilise le toucher (pétrissage, friction, tapotement)
                    <span class="dots">...</span>
                                                    <span class="more" style="display: none;">des tissus mous (muscles, tendons, ligaments) pour soulager la douleur, réduire le stress et la tension, améliorer la circulation sanguine et lymphatique.</span>
                                                </p>
                                                <button type="button" class="read-btn btn btn-sm btn-outline-primary" onclick="readMore(this)">Lire plus</button>
                                            </div>
                                        </div>
                                    </div>

                                    <!-- Projet 2 -->
                                    <div class="col-lg-4 col-md-6 project-wrap">
                                        <div class="single-project h-100">
                                            <div class="content">
                                                <a href="#" target="_blank">
                                                    <div class="content-overlay"></div>
                                                    <img class="content-image img-fluid rounded" src="../SiteUtilisateur/img/p21.jpg" alt="">
                                                </a>
                                            </div>
                                            <div class="details mt-3">
                                                <h2 class="text-uppercase">carrelage</h2>
                                                <p class="read-text">
                                                    Le carrelage est un revêtement de sol ou mural fait de carreaux (céramique, pierre, ciment) assemblés
                    <span class="dots">...</span>
                                                    <span class="more" style="display: none;">pour décorer et protéger surfaces intérieures et extérieures, offrant résistance et esthétique variée.</span>
                                                </p>
                                                <button type="button" class="read-btn btn btn-sm btn-outline-primary" onclick="readMore(this)">Lire plus</button>
                                            </div>
                                        </div>
                                    </div>

                                    <!-- Projet 3 -->
                                    <div class="col-lg-4 col-md-6 project-wrap">
                                        <div class="single-project h-100">
                                            <div class="content">
                                                <a href="#" target="_blank">
                                                    <div class="content-overlay"></div>
                                                    <img class="content-image img-fluid rounded" src="../SiteUtilisateur/img/p31.jpg" alt="">
                                                </a>
                                            </div>
                                            <div class="details mt-3">
                                                <h2 class="text-uppercase">cosmétologie</h2>
                                                <p class="read-text">
                                                    La cosmétologie est la science qui étudie les produits cosmétiques (leur composition, fabrication, effets)
                    <span class="dots">...</span>
                                                    <span class="more" style="display: none;">et leur application pour le soin, la beauté et l'embellissement de la peau, des cheveux et des ongles.</span>
                                                </p>
                                                <button type="button" class="read-btn btn btn-sm btn-outline-primary" onclick="readMore(this)">Lire plus</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Deuxième slide (3 autres projets) -->
                            <div class="carousel-item">
                                <div class="row g-4">
                                    <!-- Projet 4 -->
                                    <div class="col-lg-4 col-md-6 project-wrap">
                                        <div class="single-project h-100">
                                            <div class="content">
                                                <a href="#" target="_blank">
                                                    <div class="content-overlay"></div>
                                                    <img class="content-image img-fluid rounded" src="../SiteUtilisateur/img/p41.jpg" alt="">
                                                </a>
                                            </div>
                                            <div class="details mt-3">
                                                <h2 class="text-uppercase">cosmétologie</h2>
                                                <p class="read-text">... (texte conservé)</p>
                                                <button type="button" class="read-btn btn btn-sm btn-outline-primary" onclick="readMore(this)">Lire plus</button>
                                            </div>
                                        </div>
                                    </div>

                                    <!-- Projet 5 -->
                                    <div class="col-lg-4 col-md-6 project-wrap">
                                        <div class="single-project h-100">
                                            <div class="content">
                                                <a href="#" target="_blank">
                                                    <div class="content-overlay"></div>
                                                    <img class="content-image img-fluid rounded" src="../SiteUtilisateur/img/p51.jpg" alt="">
                                                </a>
                                            </div>
                                            <div class="details mt-3">
                                                <h2 class="text-uppercase">cosmétologie</h2>
                                                <p class="read-text">... (texte conservé)</p>
                                                <button type="button" class="read-btn btn btn-sm btn-outline-primary" onclick="readMore(this)">Lire plus</button>
                                            </div>
                                        </div>
                                    </div>

                                    <!-- Projet 6 -->
                                    <div class="col-lg-4 col-md-6 project-wrap">
                                        <div class="single-project h-100">
                                            <div class="content">
                                                <a href="#" target="_blank">
                                                    <div class="content-overlay"></div>
                                                    <img class="content-image img-fluid rounded" src="../SiteUtilisateur/img/p61.jpg" alt="">
                                                </a>
                                            </div>
                                            <div class="details mt-3">
                                                <h2 class="text-uppercase">cosmétologie</h2>
                                                <p class="read-text">... (texte conservé)</p>
                                                <button type="button" class="read-btn btn btn-sm btn-outline-primary" onclick="readMore(this)">Lire plus</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <!-- Contrôles du carousel -->
                        <button class="carousel-control-prev" type="button" data-bs-target="#optionsCarousel" data-bs-slide="prev" aria-label="Précédent">
                            <span class="d-inline-flex align-items-center justify-content-center"
                                style="width: 36px; height: 36px; background-color: rgba(0,0,0,0.5); border-radius: 50%;">
                                <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="#ffffff" stroke="#ffffff" stroke-width="1.5" viewBox="0 0 16 16">
                                    <path d="M11.354 1.646a.5.5 0 0 1 0 .708L5.707 8l5.647 5.646a.5.5 0 0 1-.708.708l-6-6a.5.5 0 0 1 0-.708l6-6a.5.5 0 0 1 .708 0" />
                                </svg>
                            </span>
                        </button>

                        <button class="carousel-control-next" type="button" data-bs-target="#optionsCarousel" data-bs-slide="next" aria-label="Suivant">
                            <span class="d-inline-flex align-items-center justify-content-center"
                                style="width: 36px; height: 36px; background-color: rgba(0,0,0,0.5); border-radius: 50%;">
                                <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="#ffffff" stroke="#ffffff" stroke-width="1.5" viewBox="0 0 16 16">
                                    <path d="M4.646 1.646a.5.5 0 0 1 .708 0l6 6a.5.5 0 0 1 0 .708l-6 6a.5.5 0 0 1-.708-.708L10.293 8 4.646 2.354a.5.5 0 0 1 0-.708" />
                                </svg>
                            </span>
                        </button>


                    </div>
                </div>
            </section>
            <!-- End project Area -->






            <!-- Start about Area -->
            <section class="about-area" id="about">
                <div class="container-fluid">
                    <div class="row d-flex justify-content-end align-items-center">
                        <div class="col-lg-6 col-md-12 about-left no-padding">
                            <img class="img-fluid" src="img/about-img.jpg" alt="">
                        </div>
                        <div class="col-lg-6 col-md-12 about-right">
                            <h1>Bienvenue à l’UTMH</h1>
                            <p>
                                L’Union des Techniciens Moderne d’Haïti est une organisation dédiée à la promotion de la technologie, la formation professionnelle
et le développement intellectuel des jeunes. Nous croyons que la technologie est un moteur essentiel pour le progrès du pays.

                            </p>
                            <!-- <button class="primary-btn mt-20 text-uppercase ">En savoir plus <span class="lnr lnr-arrow-right"></span></button>-->
                            <button type="button" class="head-btn btn text-uppercase" data-toggle="modal" data-target="#exampleLargeModal">En savoir plus <span class="lnr lnr-arrow-right"></span></button>
                        </div>
                    </div>
                </div>
            </section>
            <!-- End about Area -->

            <!-- Start volunteer Area -->
            <section class="volunteer-area section-gap">
                <div class="container">
                    <div class="row d-flex justify-content-center">
                        <div class="col-md-8 pb-80 header-text">
                            <h1>Notre Équipe</h1>
                            <p>
                                Découvrez les membres engagés qui travaillent chaque jour pour promouvoir l’excellence technique en Haïti.
 
                            </p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-3 col-md-3 vol-wrap">
                           
                        </div>
                        <div class="col-lg-3 col-md-3 vol-wrap">
                            <div class="single-vol">
                                <div class="content">
                                    <a href="#" target="_blank">
                                        <div class="content-overlay"></div>
                                        <img class="content-image img-fluid d-block mx-auto" src="../SiteUtilisateur/img/image.jpg" alt="">
                                        <div class="content-details fadeIn-bottom">
                                            <h4>Edner LEtaille <br />& <br />Jonathan JACQUES</h4>
                                            <p>
                                                Programmeur <br />& <br />Analyste
                                            </p>
                                        </div>
                                    </a>
                                </div>
                            </div>
                        </div>
                       
                      

                    </div>
                </div>
            </section>
            <!-- End volunteer Area -->


            <!-- Start Contact Area -->
            <section class="donate-area relative section-gap" id="contact">
                <div class="overlay overlay-bg"></div>
                <div class="container">

                    <!-- Titre -->
                    <div class="row d-flex justify-content-end">
                        <div class="col-lg-6 col-sm-12 pb-80 header-text">
                            <h1>Contactez-nous</h1>
                            <p>
                                Pour toute information, suggestion ou collaboration, n’hésitez pas à nous écrire.
                    L’équipe de l’UTMH vous répondra rapidement.
                            </p>
                        </div>
                    </div>

                    <div class="row d-flex justify-content-center">

                        <!-- Informations -->
                        <div class="col-lg-6 contact-left">
                            <div class="single-info">
                                <h4>Adresse</h4>
                                <p>
                                    UTMH – Union des Techniciens Moderne d’Haïti<br>
                                    Port-au-Prince, Haïti
                                </p>
                            </div>

                            <div class="single-info">
                                <h4>Email</h4>
                                <p>
                                    utmh004haiti@gmail.com<br>
                                    Réponse sous 24 heures
                                </p>
                            </div>

                            <div class="single-info">
                                <h4>Téléphone</h4>
                                <p>
                                    +509 4473-9494<br>
                                    Lundi → Dimanche
                                </p>
                            </div>
                        </div>

                        <!-- Formulaire -->
                        <div class="col-lg-6 contact-right">
                            <div class="row">

                                <div class="col-lg-6 d-flex flex-column">
                                    <asp:TextBox ID="txtNom1" class="form-control mt-20" placeholder="Votre nom complet" onfocus="this.placeholder = ''"
                                        onblur="this.placeholder = 'Votre nom'" runat="server"></asp:TextBox>
                                </div>

                                <div class="col-lg-6 d-flex flex-column">
                                    <asp:TextBox ID="txtEmail" class="form-control mt-20" placeholder="Votre email" onfocus="this.placeholder = ''"
                                        onblur="this.placeholder = 'Votre email'" runat="server"></asp:TextBox>
                                </div>

                                <!-- Champ Téléphone -->
                                <div class="col-lg-12 d-flex flex-column">
                                    <asp:TextBox ID="txtTelephone1" class="form-control mt-20" placeholder="Votre téléphone (+509XXXXXXXX)" onfocus="this.placeholder = ''"
                                        onblur="this.placeholder = 'Votre téléphone (+509XXXXXXXX)'" runat="server"></asp:TextBox>
                                </div>

                                <div class="col-lg-12 d-flex flex-column">
                                    <asp:TextBox ID="txtMessage" class="form-control mt-20" placeholder="Votre message" onfocus="this.placeholder = ''"
                                        onblur="this.placeholder = 'Votre message'" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </div>

                                <div class="col-lg-12 d-flex justify-content-end send-btn">
                                    <!-- <button class="submit-btn primary-btn mt-20 text-uppercase"> 
                            Envoyer <span class="lnr lnr-arrow-right"></span>
                        </button>-->
                                    <strong>
                                        <asp:Button ID="btnEnvoyer" runat="server" class="submit-btn primary-btn mt-20 text-uppercase" Text="Envoyer" OnClick="btnEnvoyer_Click" />
                                    </strong>

                                </div>

                                <div class="alert-msg"></div>
                            </div>
                        </div>

                    </div>
                </div>
            </section>
            <!-- End Contact Area -->



            <!-- start footer Area -->
            <footer class="footer-area section-gap">
                <div class="container">
                    <div class="row d-flex flex-column justify-content-center">
                        <ul class="footer-menu">
                            <a href="#home" class="auto-style1">Accueil</a>
                                    <a href="#project" class="auto-style1">Options</a>
                                    <a href="#about" class="auto-style1">A propos</a>
                        </ul>
                        <div class="footer-social">
                            <a href="#"><i class="fa fa-facebook"></i></a>
                            <a href="#"><i class="fa fa-twitter"></i></a>
                            <a href="#"><i class="fa fa-dribbble"></i></a>
                            <a href="#"><i class="fa fa-behance"></i></a>
                        </div>
                        <p class="footer-text m-0">
                            <!-- Link back to Colorlib can't be removed. Template is licensed under CC BY 3.0. -->
                            Copyright &copy;<script>document.write(new Date().getFullYear());</script>
                            © 2025 UTMH – Union des Techniciens Moderne d’Haïti. Tous droits réservés.
                            Développé 
                            par  <a href="#" target="_blank">Powerful Tech.</a>
                            <!-- -->
                        </p>
                    </div>
                </div>
            </footer>
            <!-- End footer Area -->
            <!-- ==================== MODAL MESSAGE ==================== -->
            <div class="modal fade" id="msgModal" tabindex="-1" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered modal-lg">
                    <div class="modal-content d-flex align-items-center p-4 border-0 shadow-lg rounded-3">

                        <!-- icon belle -->
                        <div class="me-4">
                            <svg id="msgIcon" xmlns="http://www.w3.org/2000/svg" width="50" height="50" fill="currentColor" class="text-warning" viewBox="0 0 16 16">
                                <!-- Default: alerte -->
                                <path d="M8 0a8 8 0 1 0 0 16A8 8 0 0 0 8 0zm.93 4.58a.5.5 0 0 1 .07.7L8.5 7.5v3a.5.5 0 0 1-1 0V7.5l-.5-.22a.5.5 0 1 1 .46-.88l.04.03L8 6.25l.03-.02zM8 12a1 1 0 1 1 0-2 1 1 0 0 1 0 2z" />
                            </svg>
                        </div>

                        <!-- Texte -->
                        <div class="flex-grow-1 text-start">
                            <p id="msgText" class="mb-0 text-gray-700 fs-5"></p>
                        </div>

                        <!-- Bouton OK -->
                        <div class="ms-4">
                            <button type="button" class="btn btn-primary" data-bs-dismiss="modal">OK</button>
                        </div>

                    </div>
                </div>
            </div>

            <!-- lire plus -->

            <script>
                function readMore(btn) {
                    let container = btn.previousElementSibling;
                    let dots = container.querySelector(".dots");
                    let more = container.querySelector(".more");

                    if (dots.style.display === "none") {
                        dots.style.display = "inline";
                        more.style.display = "none";
                        btn.innerHTML = "Lire plus";
                    } else {
                        dots.style.display = "none";
                        more.style.display = "inline";
                        btn.innerHTML = "Lire moins";
                    }
                }
            </script>

                    <script>
            document.addEventListener("DOMContentLoaded", function () {

                const phoneInputs = document.querySelectorAll('.phone');
                const prefix = "+509";
                const maxDigits = 8; // après +509

                function normalizePhone(input) {
                    // Récupère uniquement les chiffres tapés
                    let digits = input.value.replace(/\D/g, "");

                    // Si l'utilisateur a tapé/collé 509 au début, on l'enlève
                    if (digits.startsWith("509")) digits = digits.slice(3);

                    // Limite à 8 chiffres
                    digits = digits.slice(0, maxDigits);

                    // Reconstruit la valeur finale
                    input.value = prefix + digits;
                }

                phoneInputs.forEach(input => {

                    // Focus: impose le prefix
                    input.addEventListener("focus", () => {
                        if (!input.value.startsWith(prefix)) input.value = prefix;
                        // place le curseur à la fin
                        setTimeout(() => input.setSelectionRange(input.value.length, input.value.length), 0);
                    });

                    // Keydown: empêche de supprimer/modifier +509 + empêche non-chiffres
                    input.addEventListener("keydown", (e) => {
                        const pos = input.selectionStart;

                        // Bloquer suppression dans le prefix
                        if (pos <= prefix.length && (e.key === "Backspace" || e.key === "Delete")) {
                            e.preventDefault();
                            return;
                        }

                        // Autoriser navigation / contrôle
                        const allowed = ["ArrowLeft", "ArrowRight", "Tab", "Backspace", "Delete", "Home", "End"];
                        if (allowed.includes(e.key) || e.ctrlKey || e.metaKey) return;

                        // Après le prefix: uniquement chiffres
                        if (pos >= prefix.length && !/^\d$/.test(e.key)) {
                            e.preventDefault();
                            return;
                        }

                        // Bloquer si déjà 8 chiffres après prefix
                        const digitsCount = input.value.slice(prefix.length).replace(/\D/g, "").length;
                        if (pos >= prefix.length && digitsCount >= maxDigits) {
                            e.preventDefault();
                        }
                    });

                    // Input: sécurité totale (bloque lettres même si elles passent autrement)
                    input.addEventListener("input", () => normalizePhone(input));

                    // Paste: normalisation
                    input.addEventListener("paste", (e) => {
                        e.preventDefault();
                        const pasted = (e.clipboardData || window.clipboardData).getData("text");
                        input.value = pasted;
                        normalizePhone(input);
                    });

                    // Au chargement si champ prérempli
                    if (input.value.trim() !== "") normalizePhone(input);
                });

            });
        </script>

            <script>
                document.addEventListener("DOMContentLoaded", function () {

                    const cinInputs = document.querySelectorAll('.cin');

                    cinInputs.forEach(input => {

                        // 🔢 Autoriser seulement les chiffres et limiter à 10
                        input.addEventListener('input', function () {

                            // Supprimer tout ce qui n'est pas chiffre
                            this.value = this.value.replace(/\D/g, '');

                            // Limiter à 10 chiffres
                            if (this.value.length > 10) {
                                this.value = this.value.slice(0, 10);
                            }
                        });

                    });

                });
            </script>

            <!-- Bootstrap JS Bundle -->
            <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>



            <script src="../SiteUtilisateur/js/vendor/jquery-2.2.4.min.js"></script>
            <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.11.0/umd/popper.min.js"></script>
            <script src="../SiteUtilisateur/js/vendor/bootstrap.min.js"></script>
            <script src="../SiteUtilisateur/js/jquery.ajaxchimp.min.js"></script>
            <script src="../SiteUtilisateur/js/jquery.nice-select.min.js"></script>
            <script src="../SiteUtilisateur/js/jquery.sticky.js"></script>
            <script src="../SiteUtilisateur/js/parallax.min.js"></script>
            <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
            <script src="../SiteUtilisateur/js/jquery.magnific-popup.min.js"></script>
            <script src="../SiteUtilisateur/js/main.js"></script>

            <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.inputmask/5.0.8/jquery.inputmask.min.js"></script>
            <script>
                $(document).ready(function () {

                    // NIF Haïti : 000-000-000-0
                    $('.nif').inputmask('999-999-999-9');

                });
            </script>



        </div>
        <script>
            document.addEventListener("DOMContentLoaded", function () {

                const params = new URLSearchParams(window.location.search);

                if (params.get("open") === "register") {
                    const modalElement = document.getElementById('exampleModal');

                    if (modalElement) {
                        const modal = new bootstrap.Modal(modalElement);
                        modal.show();
                    }
                }
            });
        </script>

       <script>
           document.addEventListener("DOMContentLoaded", function () {

               const slides = document.querySelectorAll('.slide-item');
               const dotsWrap = document.getElementById('sliderDots');
               const textWrap = document.querySelector('.slider-text-wrap');
               const titleEl = document.querySelector('.slider-title');
               const subtitleEl = document.querySelector('.slider-subtitle');
               const INTERVAL = 5000;

               let current = 0;
               let timer;

               // Barre de progression
               const bar = document.createElement('div');
               bar.className = 'slider-progress';
               document.querySelector('.banner-area').appendChild(bar);

               // Création des dots
               slides.forEach((_, i) => {
                   const dot = document.createElement('button');
                   dot.className = 'slider-dot' + (i === 0 ? ' active' : '');
                   dot.setAttribute('aria-label', 'Photo ' + (i + 1));
                   dot.addEventListener('click', () => goTo(i));
                   dotsWrap.appendChild(dot);
               });

               // Mise à jour du texte avec animation
               function updateText(slide) {
                   // Cache le texte
                   textWrap.classList.remove('text-visible');

                   setTimeout(() => {
                       titleEl.textContent = slide.dataset.title || '';
                       subtitleEl.textContent = slide.dataset.subtitle || '';
                       // Réaffiche avec animation
                       textWrap.classList.add('text-visible');
                   }, 300);
               }

               function goTo(index) {
                   slides[current].classList.remove('active');
                   dotsWrap.querySelectorAll('.slider-dot')[current].classList.remove('active');

                   current = (index + slides.length) % slides.length;

                   slides[current].classList.add('active');
                   dotsWrap.querySelectorAll('.slider-dot')[current].classList.add('active');

                   updateText(slides[current]);

                   // Reset barre
                   bar.style.transition = 'none';
                   bar.style.width = '0%';
                   setTimeout(() => {
                       bar.style.transition = 'width ' + INTERVAL + 'ms linear';
                       bar.style.width = '100%';
                   }, 50);

                   clearInterval(timer);
                   timer = setInterval(() => goTo(current + 1), INTERVAL);
               }

               // Initialisation
               updateText(slides[0]);
               bar.style.transition = 'width ' + INTERVAL + 'ms linear';
               bar.style.width = '100%';
               timer = setInterval(() => goTo(current + 1), INTERVAL);
           });
       </script>

    </form>
</body>
</html>

using MauiAppMinhascompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhascompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();

        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        try
        {
            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());

        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;

            lst_produtos.IsRefreshing = true;

            lista.Clear();

            List<Produto> tmp = await App.Db.Search(q);

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        } finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);

        string msg = $"O total é {soma:C}";

        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecinado = sender as MenuItem;

            Produto p = selecinado.BindingContext as Produto;

            bool confirm = await DisplayAlert(
                "Tem Certeza?", $"Remover {p.Descricao}?", "Sim", "Não");

            if (confirm)
            {
                await App.Db.Delete(p.Id);
                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");

        } finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private async void ToolbarItem_Clicked_2(object sender, EventArgs e)
    {
        {
            try
            {
                
                var categorias = await App.Db.GetAll();  
                var categoriasDistintas = categorias
                                          .Where(p => !string.IsNullOrWhiteSpace(p.Categoria)) 
                                          .Select(p => p.Categoria)  
                                          .Distinct()  
                                          .ToList();

                string categoriaSelecionada = await DisplayActionSheet(
                    "Escolha uma categoria",
                    "Cancelar", 
                    null,       
                    categoriasDistintas.ToArray());

               
                if (categoriaSelecionada != "Cancelar")
                {
                    
                    var produtosPorCategoria = categorias.Where(p => p.Categoria == categoriaSelecionada).ToList();

                    
                    double totalCategoria = produtosPorCategoria.Sum(p => (double)(p.Quantidade * p.Preco));

                    
                    await DisplayAlert("Total Gasto", $"O total gasto na categoria {categoriaSelecionada} é {totalCategoria:C}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}